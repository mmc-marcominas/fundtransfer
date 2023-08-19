using System.Text.Json.Serialization;

namespace FundTransfer;
/// <summary>
/// TransferStatus response
/// </summary>
public class TransferStatusResponse
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
