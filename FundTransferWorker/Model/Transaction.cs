using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace FundTransferWorker;
/// <summary>
/// Transaction
/// </summary>
public class Transaction
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

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
    /// <summary>
    /// Transaction status
    /// </summary>
    public string Status { get; set; }
    /// <summary>
    /// Last error message
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string LastError { get; set; }
}
