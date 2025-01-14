using MarketPlace.Models;
using MarketPlace.Services.IServices;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using static MarketPlace.Utility.SD;

namespace MarketPlace.Services
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenProvider _tokenProvider;
        public BaseService(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider)
        {
            _httpClientFactory= httpClientFactory;
            _tokenProvider= tokenProvider;
        }
        public async Task<ResponseDto?> SendAsync(RequestDto requestDto, bool withBearer = true)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("MarketPlaceAPI");
                HttpRequestMessage message= new();

                //token Authorization
                if(withBearer)
                {
                    var token = _tokenProvider.GetToken();
                    message.Headers.Add("Authorization", $"Bearer {token}");
                }
                //Url
                message.RequestUri = new Uri(requestDto.Url);

                if (requestDto.Data != null)
                {
                    //body type
                    message.Headers.Add("Accept", "application/json");
                    //body content
                    message.Content=new StringContent(JsonConvert.SerializeObject(requestDto.Data),Encoding.UTF8,"application/json");
                }

                //body type
                message.Headers.Add("Accept", "application/json");

                switch (requestDto.ApiType)
                {
                    case ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                HttpResponseMessage? apiResponse = null;

                apiResponse= await client.SendAsync(message);

                switch (apiResponse.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return new()
                        {
                            IsSuccess = false,
                            Message = "Not Found"
                        };
                    case HttpStatusCode.Forbidden:
                        return new()
                        {
                            IsSuccess = false,
                            Message = "Access Denied"
                        };
                    case HttpStatusCode.Unauthorized:
                        return new()
                        {
                            IsSuccess = false,
                            Message = "Unauthorized"
                        };
                    case HttpStatusCode.BadRequest:
                        return new()
                        {
                            IsSuccess = false,
                            Message = "Bad Request"
                        };
                    case HttpStatusCode.InternalServerError:
                        return new()
                        {
                            IsSuccess = false,
                            Message = "Internal Server Error"
                        };
                    default:
                        var apiContent = await apiResponse.Content.ReadAsStringAsync();
                        var apiReponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                        return apiReponseDto;
                }

            }
            catch (Exception ex) {
                var dto = new ResponseDto
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
                return dto;
            }
        }
    }
}
