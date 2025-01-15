using MarketPlace.Cart.Models.Dto;

namespace MarketPlace.Cart.Services.IServices
{
    public interface ICouponService
    {
        Task<CouponDto> GetCoupons(string couponCode);
    }
}
