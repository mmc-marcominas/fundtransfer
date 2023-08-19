namespace FundTransfer.Services;

/// <summary>
/// Transfer Service Interface
/// </summary>
public interface ITransferService
{
    /// <summary>
    /// Initiate a fund transfer.
    /// </summary>
    /// <param name="transaction" >Transaction data .</param>
    /// <returns>TransactionId</returns>
    string InitiateTransfer(Transaction transaction);
    /// <summary>
    /// Get transfer status of a transactionId specified
    /// </summary>
    /// <param name="transactionId">TransactionId</param>
    /// <returns><see cref="TransferStatus"/> of specified transactionId</returns>
    TransferStatusResponse GetTransferStatus(string transactionId);
}

public class TransferService : ITransferService
{
    private readonly Dictionary<string, TransferStatus> _transactionStatuses = new();
    private readonly List<Transaction> _transactions = new();

    /// <summary>
    /// <inheritdoc cref="ITransferService.InitiateTransfer(Transaction)"/>
    public string InitiateTransfer(Transaction transaction)
    {
        var transactionId = Guid.NewGuid().ToString();

        _transactions.Add(transaction);
        _transactionStatuses[transactionId] = TransferStatus.Processing;

        return transactionId;
    }

    /// <summary>
    /// <inheritdoc cref="ITransferService.GetTransferStatus(string)"/>
    public TransferStatusResponse GetTransferStatus(string transactionId)
    {
        if (_transactionStatuses.TryGetValue(transactionId, out var status))
        {
            // Implement logic to return status response
            if (status == TransferStatus.Error)
            {
                return new TransferStatusResponse
                {
                    Status = TransferStatus.Error.GetDescription(),
                    Message = "Invalid account number"
                };
            }
            return new TransferStatusResponse { Status = status.GetDescription() };
        }
        
        return new TransferStatusResponse
        {
            Status = TransferStatus.Error.GetDescription(),
            Message = "Transaction not found"
        };
    }
}
