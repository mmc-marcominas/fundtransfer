namespace FundTransfer;

/// <summary>
/// Database settings
/// </summary>
public class DatabaseSettings
{
    /// <summary>
    /// Connection string
    /// </summary>
    public string ConnectionString { get; set; } = null!;

    /// <summary>
    /// Database name
    /// </summary>
    public string DatabaseName { get; set; } = null!;

    /// <summary>
    /// Collection name
    /// </summary>
    public string CollectionName { get; set; } = null!;
}