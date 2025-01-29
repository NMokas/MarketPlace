using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MarketPlace.EmailBrokerAPI.Models;
using Microsoft.AspNetCore.Connections;
using System.Text.Json;
using System.Text;
using RabbitMQ.Client;
namespace MarketPlace.EmailBrokerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailBrokerAPIController : ControllerBase
    {
        private ResponseDto _response;
        private ConnectionFactory _connectionFactory;
        private readonly IConfiguration _configuration;
        public EmailBrokerAPIController(IConfiguration configuration)
        {
            this._response = new ResponseDto();
            _configuration = configuration;
            this._connectionFactory = new ConnectionFactory();
        }

        [HttpPost]
        public async Task<ResponseDto> SendMessageToBroker([FromBody]EmailDto emailDto)
        {
            try
            {
                _connectionFactory.Uri = new Uri(_configuration["BrokerSettings:factoryUri"]);
                _connectionFactory.ClientProvidedName = "Rabbit Sender App";

                IConnection cnn = await _connectionFactory.CreateConnectionAsync();

                IChannel channel = await cnn.CreateChannelAsync();

                string exchangeName = _configuration["BrokerSettings:exchangeName"];
                string routingKey = _configuration["BrokerSettings:routingKey"];
                string queueName = _configuration["BrokerSettings:queueName"];

                await channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Direct);
                await channel.QueueDeclareAsync(queueName, false, false, false, null);
                await channel.QueueBindAsync(queueName, exchangeName, routingKey, null);

                foreach(var email in emailDto.Emails)
                {   
                    var emailData = new
                    {
                        To = email,
                        Subject = emailDto.Subject,
                        Body = emailDto.Content,
                        Attachments = new string[] { }
                    };
                    var messageBody = JsonSerializer.Serialize(emailData);
                    var body = Encoding.UTF8.GetBytes(messageBody);

                    await channel.BasicPublishAsync(exchange: exchangeName,
                                                        routingKey: routingKey,
                                                        body: body);

                    Console.WriteLine($" [x] Sent email to {emailData.To}");
                }
                await channel.CloseAsync();
                await cnn.CloseAsync();
                _response.Result = "Message sent with success";
            }
            catch(Exception ex) {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }
    }
}
