using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using FundTransferWorker.Services;
using Microsoft.Extensions.Options;

namespace FundTransferWorker;
public class FundTransferService
{
  private readonly ILogger<FundTransferService> _logger;
  private readonly IHttpClientFactory _httpClientFactory;
  private readonly TransactionsDatabaseService _databaseService;
  private readonly AccountApiSettings _accountApiSettings;

  public FundTransferService(
    ILogger<FundTransferService> logger,
    IHttpClientFactory httpClientFactory,
    IOptions<AccountApiSettings> accountApiSettings,
    TransactionsDatabaseService databaseService)
  {
    _logger = logger;
    _databaseService = databaseService;
    _httpClientFactory = httpClientFactory;
    _accountApiSettings = accountApiSettings.Value;
  }

  public async Task ProcessTransferAsync(Transaction transaction)
  {
    var accountValidationResponse = await ValidateAccountAsync(transaction);

    if (accountValidationResponse.IsValid)
    {
      var accountExecutionResponse = await ExecuuteTransferAsync(transaction, accountValidationResponse);

      if (accountExecutionResponse.IsValid)
      {
        await UpdateTransactionStatusAsync(transaction, "Confirmed");
        _logger.LogInformation($"FundTransferService.ProcessTransferAsync Success: {transaction.TransactionId}");
      }
      else
      {
        await UpdateTransactionStatusAsync(transaction, "Error", accountExecutionResponse.Message);
        _logger.LogError($"FundTransferService.ProcessTransferAsync Error: {accountExecutionResponse.Message}");
      }
    }
    else
    {
      await UpdateTransactionStatusAsync(transaction, "Error", accountValidationResponse.Message);
      _logger.LogError($"FundTransferService.ProcessTransferAsync Error: {accountValidationResponse.Message}");
    }
  }

  private async Task<AccountValidationResponse> ValidateAccountAsync(Transaction transaction)
  {
    // TODO: use of retry policy do improve success on execution requests
    var httpClient = GetHttpClient();
    var response = await httpClient.GetAsync($"{httpClient.BaseAddress.AbsoluteUri}/{transaction.AccountOrigin}");

    if (!response.IsSuccessStatusCode)
    {
      return AccountValidationResponse.GetError("Problem when retrieving origin account");
    }

    var content = await response.Content.ReadAsStringAsync();
    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    var accountResponseOrigin = JsonSerializer.Deserialize<AccountResponse>(content, options);
    if (transaction.Value > accountResponseOrigin?.Balance)
    {
      return AccountValidationResponse.GetError("Insuficient fund on origin account");
    }

    response = await httpClient.GetAsync($"{httpClient.BaseAddress.AbsoluteUri}/{transaction.AccountDestination}");
    if (!response.IsSuccessStatusCode)
    {
      return AccountValidationResponse.GetError("Problem when retrieving destination account");
    }

    content = await response.Content.ReadAsStringAsync();
    var accountResponseDestination = JsonSerializer.Deserialize<AccountResponse>(content, options);

    return new AccountValidationResponse
    {
      IsValid = true,
      Origin = accountResponseOrigin,
      Destination = accountResponseDestination
    };
  }
  private async Task<AccountValidationResponse> ExecuuteTransferAsync(Transaction transaction, AccountValidationResponse accountValidationResponse)
  {
    if (accountValidationResponse == default ||
        accountValidationResponse.Origin == default ||
        accountValidationResponse.Destination == default)
    {
      var validation = accountValidationResponse == default;
      var origin = accountValidationResponse == default;
      var destination = accountValidationResponse == default;

      return AccountValidationResponse.GetError($"Problem when updating accounts: v: {validation}, o: {origin}, d: {destination}");
    }

    accountValidationResponse.Origin.Balance -= transaction.Value;
    accountValidationResponse.Destination.Balance += transaction.Value;

    var originUpdate = await UpdateAccount(accountValidationResponse.Origin);

    if (originUpdate.IsValid)
    {
      var destinationUpdate = await UpdateAccount(accountValidationResponse.Destination);
      if (destinationUpdate.IsValid)
      {
        return new AccountValidationResponse { IsValid = true };
      }

      accountValidationResponse.Origin.Balance += transaction.Value;
      originUpdate = await UpdateAccount(accountValidationResponse.Origin);

      if (!originUpdate.IsValid)
      {
        // TODO: thing in a way to lead with this.
        _logger.LogError($"FundTransferService.ProcessTransferAsync Error rollbacking origin balance update");
      }
    }

    return AccountValidationResponse.GetError("Error on transfer accounts execution");
  }

  private static StringContent GetStringContent(AccountResponse account) =>
                        new(JsonSerializer.Serialize(account), Encoding.UTF8);

  private async Task<AccountValidationResponse> UpdateAccount(AccountResponse accountResponse)
  {
    var request = new HttpRequestMessage(HttpMethod.Post, "Account");
    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    request.Content = GetStringContent(accountResponse);
    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

    var httpClient = GetHttpClient();
    var response = await httpClient.SendAsync(request);

    if (!response.IsSuccessStatusCode)
    {
      return AccountValidationResponse.GetError("Problem when updating account");
    }

    return new AccountValidationResponse { IsValid = true };
  }

  private async Task UpdateTransactionStatusAsync(Transaction transaction, string status, string? lastError = null)
  {
    var document = await _databaseService.GetAsync(transaction.TransactionId);
    if (document != null && document.Id != default)
    {
      document.Status = status;
      if (!string.IsNullOrWhiteSpace(lastError))
      {
        document.LastError = lastError;
      }
      await _databaseService.UpdateAsync(document.Id.ToString(), document);
    }
  }

  private HttpClient GetHttpClient()
  {
    // TODO: use of retry policy do improve success on execution requests
    var httpClient = _httpClientFactory.CreateClient();
    var uri = new Uri(_accountApiSettings.AccountValidationEndpoint);

    httpClient.BaseAddress = uri;

    return httpClient;
  }
}
