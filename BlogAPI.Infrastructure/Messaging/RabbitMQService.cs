using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BlogAPI.Infrastructure.Messaging;

public class RabbitMQService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly string _queueName;
    
    public RabbitMQService(IConfiguration configuration)
    {
        var factory = new ConnectionFactory()
        {
            HostName = configuration["RabbitMQ:HostName"],
            Port = int.Parse(configuration["RabbitMQ:Port"]),
            UserName = configuration["RabbitMQ:UserName"],
            Password = configuration["RabbitMQ:Password"]
        };

        _queueName = configuration["RabbitMQ:QueueName"];
            
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
            
        _channel.QueueDeclare(
            queue: _queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);
    }
    
    public void PublishMessage<T>(T message)
    {
        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        _channel.BasicPublish(
            exchange: "",
            routingKey: _queueName,
            basicProperties: null,
            body: body);
    }
    
    public void ConsumeMessages<T>(Action<T> processMessage)
    {
        var consumer = new EventingBasicConsumer(_channel);
            
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);
            var message = JsonSerializer.Deserialize<T>(json);
                
            processMessage(message);
                
            _channel.BasicAck(ea.DeliveryTag, false);
        };

        _channel.BasicConsume(
            queue: _queueName,
            autoAck: false,
            consumer: consumer);
    }
}