using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace FundTransfer.Services;

public interface ITransactionsQueueService
{
  public void Enqueue(Transaction transaction);
}

public class TransactionsQueueService : ITransactionsQueueService
{
  private readonly QueueSettings _queueSettings;
  private IConnection _rabbitMQConnection;

  public TransactionsQueueService(IOptions<QueueSettings> queueSettings)
  {
    _queueSettings = queueSettings.Value;

    var factory = new ConnectionFactory
    {
      Uri = new Uri(_queueSettings.ConnectionString)
    };
    _rabbitMQConnection = factory.CreateConnection();
  }

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
