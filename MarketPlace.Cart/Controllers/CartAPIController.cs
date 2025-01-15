using AutoMapper;
using MarketPlace.Cart.Data;
using MarketPlace.Cart.Models;
using MarketPlace.Cart.Models.Dto;
using MarketPlace.Cart.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Cart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartAPIController : ControllerBase
    {
        private ResponseDto _response;
        private IMapper _mapper;
        private readonly ApplicationDbContext _db;
        private IProductService _productService;//product service
        private ICouponService _couponService;//coupon service
        //service bus for email notifications

        public CartAPIController(IMapper mapper, ApplicationDbContext db, ICouponService couponService,IProductService productService)
        {
            this._response = new ResponseDto();
            _mapper = mapper;
            _db = db;
            _productService=productService;
            _couponService = couponService;

        }

        [HttpPost("CartUpsert")]
        public async Task<ResponseDto>CartUpsert(CartDto cartDto)
        {
            try
            {
                var cartHeaderFromDb = await _db.CartHeaders.AsNoTracking().FirstOrDefaultAsync(u=>u.UserId==cartDto.CartHeader.UserId);
                if (cartHeaderFromDb == null)
                {
                    //Create Header and Details
                    CartHeader cartHeader=_mapper.Map<CartHeader>(cartDto.CartHeader);
                    _db.CartHeaders.Add(cartHeader);
                    await _db.SaveChangesAsync();
                    cartDto.CartDetails.First().CartHeaderId=cartHeader.CartHeaderId;
                    _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                }
                else
                {
                    //if header is not null
                    //check if details has same product
                    var cartDetailsFromDb = await _db.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                        x => x.ProductId == cartDto.CartDetails.First().ProductId 
                        && x.CartHeaderId==cartHeaderFromDb.CartHeaderId);
                    if(cartDetailsFromDb == null)
                    {
                        cartDto.CartDetails.First().CartHeaderId=cartHeaderFromDb.CartHeaderId;
                        _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        cartDetailsFromDb.Count = +cartDto.CartDetails.First().Count;
                        //cartDetailsFromDb.CartHeaderId = cartDto.CartHeader.CartHeaderId;
                        //cartDetailsFromDb.CartDetailsId=cartDto.CartDetails.First().CartHeaderId;
                        _db.CartDetails.Update(cartDetailsFromDb);
                        await _db.SaveChangesAsync();
                    }
                }
                _response.Result = cartDto;

            }catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }
    }
}
