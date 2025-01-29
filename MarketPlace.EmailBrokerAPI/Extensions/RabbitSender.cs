using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using System.Threading.Channels;

namespace MarketPlace.EmailBrokerAPI.Extensions
{
    public class RabbitSender:IAsyncDisposable
    {
        private ConnectionFactory _connectionFactory;
        private readonly string _exchangeName;
        private readonly string _routingKey;
        private readonly string _queueName;
        private IConnection _connection;
        private IChannel _channel;

        public RabbitSender(IConfiguration configuration)
        {
            _connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(configuration["BrokerSettings:factoryUri"]),
                ClientProvidedName = "Rabbit Sender App"
            };
            _exchangeName = configuration["BrokerSettings:exchangeName"];
            _routingKey = configuration["BrokerSettings:routingKey"];
            _queueName = configuration["BrokerSettings:queueName"];
        }

        public async Task InitializeAsync()
        {
            _connection = await _connectionFactory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            await _channel.ExchangeDeclareAsync(_exchangeName, ExchangeType.Direct);
            await _channel.QueueDeclareAsync(_queueName, false, false, false, null);
            await _channel.QueueBindAsync(_queueName, _exchangeName, _routingKey, null);
        }
        public async Task PublishMessageAsync(object message)
        {
            if (_channel is null)
            {
                throw new InvalidOperationException("RabbitMQ service not initialized.");
            }

            var messageBody = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(messageBody);

            
            var options = new BasicProperties
            {
                Persistent = true // ✅ Correct way to make messages durable
            };

            await _channel.BasicPublishAsync(exchange: _exchangeName,
                                             routingKey: _routingKey,
                                             mandatory:true,
                                             basicProperties: options,
                                             body: body);
        }
        public async ValueTask DisposeAsync()
        {
            if (_channel != null)
            {
                await _channel.CloseAsync();
                _channel.Dispose();
            }
            if (_connection != null)
            {
                await _connection.CloseAsync();
                _connection.Dispose();
            }
        }

    }

}
