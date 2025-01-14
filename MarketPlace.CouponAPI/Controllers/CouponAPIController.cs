using AutoMapper;
using MarketPlace.CouponAPI.Data;
using MarketPlace.CouponAPI.Models;
using MarketPlace.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.CouponAPI.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    public class CouponAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private ResponseDto _response;
        public CouponAPIController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDto();
        }
        [Authorize]
        [HttpGet]
        public ResponseDto GetCoupons()
        {

            var coupons =  _db.Coupons.ToList();
            if (coupons != null)
            {
                _response.Result = _mapper.Map<IEnumerable<CouponDto>>(coupons);
            }
            else
            {
                _response.IsSuccess = false;
                _response.Message = "There is no coupons available at this moment";
            }


            return _response;
        }
        [Authorize]
        [HttpGet]
        [Route("id:int")]

        public async Task<ResponseDto> GetCouponById(int id)
        {

            var coupon = await _db.Coupons.FirstOrDefaultAsync(x=>x.Id==id);
            if (coupon != null)
            {
                _response.Result = _mapper.Map<CouponDto>(coupon);
            }
            else
            {
                _response.IsSuccess = false;
                _response.Message = "This coupon does not exists";
            }


            return _response;
        }
        [HttpDelete]
        [Authorize]
        [Route("id:int")]

        public async Task<ResponseDto> DeleteCoupon(int id)
        {

            var coupon = await _db.Coupons.FirstOrDefaultAsync(x => x.Id == id);
            if (coupon != null)
            {
                _db.Coupons.Remove(coupon);
                _db.SaveChanges();
                _response.Result = "Coupon Deleted with success";
            }
            else
            {
                _response.IsSuccess = false;
                _response.Message = "This coupon does not exists";
            }


            return _response;
        }
        [HttpGet]
        [Authorize]
        [Route("couponCode:string")]

        public async Task<ResponseDto> GetCouponByCode(string couponCode)
        {

            var coupon = await _db.Coupons.FirstOrDefaultAsync(x => x.Code == couponCode);
            if (coupon != null)
            {
                _response.Result = _mapper.Map<CouponDto>(coupon);
            }
            else
            {
                _response.IsSuccess = false;
                _response.Message = "This coupon does not exists";
            }


            return _response;
        }
        [Authorize]
        [HttpPut]
        public async Task<ResponseDto> UpdateCoupon([FromBody] CouponDto couponDto)
        {

            var auxCoupon = _db.Coupons.FirstOrDefault(x => x.Id == couponDto.Id);
            if (auxCoupon != null)
            {
                var coupon = _mapper.Map<Coupon>(auxCoupon);
                _db.Coupons.Update(coupon);
                await _db.SaveChangesAsync();
                _response.Result = coupon;
            }
            else
            {
                _response.IsSuccess = false;
                _response.Message = "This coupon does not exists";
            }

            return _response;

        }
        [Authorize]
        [HttpPost]
        public async Task<ResponseDto> CreateCoupon([FromBody] CouponDto couponDto)
        {
            if (couponDto != null)
            {
                var coupon = _mapper.Map<Coupon>(couponDto);
                _db.Coupons.Add(coupon);
                await _db.SaveChangesAsync();
                _response.Result = coupon;
            }
            else
            {
                _response.IsSuccess = false;
                _response.Message = "This coupon does not exists";
            }

            return _response;

        }

    }
}
