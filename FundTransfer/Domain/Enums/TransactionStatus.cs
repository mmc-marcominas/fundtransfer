namespace FundTransfer;

/// <summary>
/// Transaction statuses
/// </summary>
public enum TransactionStatus
{
    /// <summary>
    /// In queue
    /// </summary>
    InQueue,
    /// <summary>
    /// Processing
    /// </summary>
    Processing,
    /// <summary>
    /// Confirmed
    /// </summary>
    Confirmed,
    /// <summary>
    /// Error
    /// </summary>
    Error
}

/// <summary>
/// TransferStatus extensions
/// </summary>
public static class TransferStatusExtensions
{
    /// <summary>
    /// Get TransferStatus description
    /// </summary>
    /// <param name="transactionStatus"><see cref="TransactionStatus"/></param>
    /// <returns>String representation of <see cref="TransactionStatus"/></returns>
    public static string GetDescription(this TransactionStatus transactionStatus)
    {
        return transactionStatus switch
        {
            TransactionStatus.InQueue => "In queue",
            TransactionStatus.Processing => "Processing",
            TransactionStatus.Confirmed => "Confirmed",
            TransactionStatus.Error => "Error",
            _ => "Undefined",
        };
    }
}
