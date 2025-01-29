using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MarketPlace.EmailBrokerAPI.Models;
using Microsoft.AspNetCore.Connections;
using System.Text.Json;
using System.Text;
using RabbitMQ.Client;
using System.Net.NetworkInformation;
using MarketPlace.EmailBrokerAPI.Extensions;
namespace MarketPlace.EmailBrokerAPI.Controllers
{
    [Route("api/RabbitBrokerSender")]
    [ApiController]
    public class EmailBrokerAPIController : ControllerBase
    {
        private ResponseDto _response;
        private readonly RabbitSender _rabbitMqService;

        public EmailBrokerAPIController(RabbitSender rabbitMqService)
        {
            _response = new ResponseDto();
            _rabbitMqService = rabbitMqService;
        }

        [HttpPost]
        public async Task<ResponseDto> SendMessageToBroker([FromBody]EmailDto emailDto)
        {
            try
            {
                foreach(var email in emailDto.Emails)
                {
                    var emailData = new
                    {
                        To = email,
                        Subject = emailDto.Subject,
                        Body = emailDto.Content,
                        Attachments = new string[] { }
                    };

                    await _rabbitMqService.PublishMessageAsync(emailData);
                    Console.WriteLine($" [x] Sent email to {email}");
                }
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
