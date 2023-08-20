namespace FundTransferWorker;
public class QueueSettings
{
    public string ConnectionString { get; set; } = null!;

    public string Exchange { get; set; }

    public string QueueName { get; set; } = null!;

    public string CollectionName { get; set; } = null!;

    public int TaskDelayMiliseconds { get; set; } = 0;
}
