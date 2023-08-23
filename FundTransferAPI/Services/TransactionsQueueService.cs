using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace FundTransfer.Services;

/// <summary>
/// Transactions Queue Service interface
/// </summary>
public interface ITransactionsQueueService
{
  /// <summary>
  /// Enqueue
  /// </summary>
  /// <param name="transaction"></param>
  public void Enqueue(Transaction transaction);
}

/// <summary>
/// Transactions Queue Service implementation
/// </summary>
public class TransactionsQueueService : ITransactionsQueueService
{
  private readonly QueueSettings _queueSettings;
  private IConnection _rabbitMQConnection;

  /// <summary>
  ///  Transactions Queue Service constructor
  /// </summary>
  /// <param name="queueSettings"><see cref="IOptions<QueueSettings>"/></param>
  public TransactionsQueueService(IOptions<QueueSettings> queueSettings)
  {
    _queueSettings = queueSettings.Value;

    var factory = new ConnectionFactory
    {
      Uri = new Uri(_queueSettings.ConnectionString)
    };
    _rabbitMQConnection = factory.CreateConnection();
  }

  /// <summary>
  /// Enqueue
  /// </summary>
  /// <param name="transaction"></param>
  public void Enqueue(Transaction transaction)
  {
    using var channel = _rabbitMQConnection.CreateModel();
    channel.QueueDeclare(queue: _queueSettings.QueueName,
                         durable: true,
                         exclusive: false,
                         autoDelete: false,
                         arguments: null);

    var message = JsonSerializer.Serialize(transaction);
    var body = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(exchange: _queueSettings.Exchange,
                         routingKey: _queueSettings.QueueName,
                         basicProperties: null,
                         body: body);
  }
}
