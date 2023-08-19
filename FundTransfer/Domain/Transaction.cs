namespace FundTransfer;
/// <summary>
/// Transaction
/// </summary>
public class Transaction
{
    /// <summary>
    /// Transaction id
    /// </summary>
    public string TransactionId { get; set; }
    /// <summary>
    /// Account origin
    /// </summary>
    public string AccountOrigin { get; set; }
    /// <summary>
    /// Account destination
    /// </summary>
    public string AccountDestination { get; set; }
    /// <summary>
    /// Transaction value
    /// </summary>
    public decimal Value { get; set; }
}
