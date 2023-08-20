/// <summary>
/// Account
/// </summary>
public class AccountResponse
{
    /// <summary>
    /// Account number
    /// </summary>
    public string AccountNumber { get; set; }
    /// <summary>
    /// Account balance
    /// </summary>
    public decimal Balance { get; set; }
}
public class AccountValidationResponse
{
    public bool IsValid { get; set; }
    public string? Message { get; set; }
    public AccountResponse? Origin { get; set; }
    public AccountResponse? Destination { get; set; }

    public static AccountValidationResponse GetError(string message)
    {
        return new AccountValidationResponse
        {
            IsValid = false,
            Message = message
        };
    }
}
