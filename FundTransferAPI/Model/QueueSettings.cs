namespace FundTransfer;

/// <summary>
/// Queue settings
/// </summary>
public class QueueSettings
{
    /// <summary>
    /// Connection string
    /// </summary>
    public string ConnectionString { get; set; } = null!;

    /// <summary>
    /// Exchange name
    /// </summary>
    public string Exchange { get; set; }

    /// <summary>
    /// Queue name
    /// </summary>
    public string QueueName { get; set; } = null!;

    /// <summary>
    /// Collection name
    /// </summary>
    public string CollectionName { get; set; } = null!;
}
