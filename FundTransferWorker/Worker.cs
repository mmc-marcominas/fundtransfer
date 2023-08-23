using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace FundTransferWorker;

/// <summary>
/// Background Service FundTransfer Worker
/// </summary>
public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly QueueSettings _queueSettings;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private IConnection _rabbitMQConnection = null!;

    /// <summary>
    /// Background Service FundTransfer Worker constructor
    /// </summary>
    /// <param name="logger"><see cref="ILogger<Worker>"/></param>
    /// <param name="serviceScopeFactory"><see cref="IServiceScopeFactory"/></param>
    /// <param name="queueSettings"><see cref="IOptions<QueueSettings>"/></param>
    public Worker(
        ILogger<Worker> logger,
        IServiceScopeFactory serviceScopeFactory,
        IOptions<QueueSettings> queueSettings)
    {
        _logger = logger;
        _queueSettings = queueSettings.Value;
        _serviceScopeFactory = serviceScopeFactory;
        InitializeRabbitMQConnection();
    }

    /// <summary>
    /// Initialize RabbitMQ connection
    /// </summary>
    private void InitializeRabbitMQConnection()
    {
        var factory = new ConnectionFactory
        {
            Uri = new Uri(_queueSettings.ConnectionString)
        };
        _rabbitMQConnection = factory.CreateConnection();
    }

    /// <summary>
    /// ExecuteAsync implementation
    /// </summary>
    /// <param name="stoppingToken"><see cref="CancellationToken"/></param>
    /// <returns></returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var channel = _rabbitMQConnection.CreateModel();
        var queueName = _queueSettings.QueueName;
        channel.QueueDeclare(queue: queueName,
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, eventArguments) =>
        {
            var body = eventArguments.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            using var scope = _serviceScopeFactory.CreateScope();
            var fundTransferService = scope.ServiceProvider.GetRequiredService<FundTransferService>();

            var transaction = JsonSerializer.Deserialize<Transaction>(message);
            if (transaction == default)
            {
                _logger.LogError("Worker.ExecuteAsync: Invalid payload");
                return;
            }
            await fundTransferService.ProcessTransferAsync(transaction);

            channel.BasicAck(deliveryTag: eventArguments.DeliveryTag, multiple: false);
        };

        channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(_queueSettings.TaskDelayMiliseconds, stoppingToken);
        }
    }

    public override void Dispose()
    {
        base.Dispose();
        _rabbitMQConnection?.Close();
        _rabbitMQConnection?.Dispose();
    }
}
