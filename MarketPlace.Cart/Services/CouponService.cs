using MarketPlace.Cart.Models.Dto;
using MarketPlace.Cart.Services.IServices;
using Newtonsoft.Json;

namespace MarketPlace.Cart.Services
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public CouponService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<CouponDto> GetCoupons(string couponCode)
        {
            var client = _httpClientFactory.CreateClient("Coupon");//Create a new object of the type httpclient called Coupon
            var response = await client.GetAsync($"/api/coupon/{couponCode}"); //Call to the API using GetAsync who enables a async call
            var apiContent= await response.Content.ReadAsStringAsync();//Converts the response body into a string
            var resp=JsonConvert.DeserializeObject<ResponseDto>(apiContent);//Converts the previous string to a ResponseDto type
            if(resp.IsSuccess)
            {
                return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(resp.Result));//converts the ResponseDto.Result to the expected object received
            }
            return new CouponDto();//Else returns a null and new coupon dto
        }
    }
}
