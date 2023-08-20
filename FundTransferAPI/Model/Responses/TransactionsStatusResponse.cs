using System.Text.Json.Serialization;

namespace FundTransfer;
/// <summary>
/// TransferStatus response
/// </summary>
public class TransactionsStatusResponse
{
    /// <summary>
    /// Status
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Status { get; set; }
    /// <summary>
    /// Message
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Message { get; set; }
}
