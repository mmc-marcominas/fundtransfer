namespace FundTransfer;

/// <summary>
/// Transfer statuses
/// </summary>
public enum TransferStatus
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
    /// <param name="transferStatus"><see cref="TransferStatus"/></param>
    /// <returns>String representation of <see cref="TransferStatus"/></returns>
    public static string GetDescription(this TransferStatus transferStatus)
    {
        var description = transferStatus switch
        {
            TransferStatus.InQueue => "In queue",
            TransferStatus.Processing => "Processing",
            TransferStatus.Confirmed => "Confirmed",
            TransferStatus.Error => "Error",
            _ => "Undefined",
        };

        return description;
    }
}
