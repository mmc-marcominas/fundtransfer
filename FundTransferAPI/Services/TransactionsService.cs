namespace FundTransfer.Services;

/// <summary>
/// Transaction Service Interface
/// </summary>
public interface ITransactionsService
{
    /// <summary>
    /// Initiate a fund transfer.
    /// </summary>
    /// <param name="transaction" >Transaction data .</param>
    /// <returns>TransactionId</returns>
    Task<string> InitiateTransfer(Transaction transaction);
    /// <summary>
    /// Get transaction status of a transactionId specified
    /// </summary>
    /// <param name="transactionId">TransactionId</param>
    /// <returns><see cref="TransactionStatus"/> of specified transactionId</returns>
    Task<TransactionsStatusResponse> GetTransactionStatus(string transactionId);
}

public class TransactionsService : ITransactionsService
{
    private readonly ITransactionsQueueService _transactionsQueueService;
    private readonly TransactionsDatabaseService _databaseService;

    public TransactionsService(TransactionsDatabaseService databaseService, ITransactionsQueueService transactionsQueueService)
    {
        _databaseService = databaseService;
        _transactionsQueueService = transactionsQueueService;
    }
            

    /// <summary>
    /// <inheritdoc cref="ITransactionsService.InitiateTransfer(Transaction)"/>
    public async Task<string> InitiateTransfer(Transaction transaction)
    {
        transaction.TransactionId = Guid.NewGuid().ToString();
        transaction.Status = TransactionStatus.InQueue.GetDescription();

        await _databaseService.CreateAsync(transaction);
        _transactionsQueueService.Enqueue(transaction);

        return transaction.TransactionId;
    }

    /// <summary>
    /// <inheritdoc cref="ITransactionsService.GetTransactionStatus(string)"/>
    public async Task<TransactionsStatusResponse> GetTransactionStatus(string transactionId)
    {

        var transaction = await _databaseService.GetAsync(transactionId);

        if (transaction is null)
        {
            return new TransactionsStatusResponse
            {
                Status = TransactionStatus.Error.GetDescription(),
                Message = "Transaction not found"
            };
        }

        // Implement logic to return status response
        if (transaction.Status.Equals(TransactionStatus.Error.GetDescription(), StringComparison.OrdinalIgnoreCase))
        {
            return new TransactionsStatusResponse
            {
                Status = TransactionStatus.Error.GetDescription(),
                Message = transaction.LastError
            };
        }

        return new TransactionsStatusResponse { Status = transaction.Status };
    }
}
