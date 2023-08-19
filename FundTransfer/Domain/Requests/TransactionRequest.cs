namespace FundTransfer;

/// <summary>
/// Transaction
/// </summary>
public class TransactionRequest
{
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

/// <summary>
/// TransactionRequest extensions
/// </summary>
public static class TransactionRequestExtensions
{
    /// <summary>
    /// Get <see cref="Transaction"/> from a <see cref="TransactionRequest"/>
    /// </summary>
    /// <param name="transactionRequest"><see cref="TransactionRequest"/></param>
    /// <returns>A <see cref="Transaction"/> with <see cref="TransactionRequest"/> values</returns>
    public static Transaction GetTransaction(this TransactionRequest transactionRequest)
    {
      return new Transaction
      {
        AccountOrigin = transactionRequest.AccountOrigin,
        AccountDestination = transactionRequest.AccountDestination,
        Value = transactionRequest.Value
      };
    }

}
