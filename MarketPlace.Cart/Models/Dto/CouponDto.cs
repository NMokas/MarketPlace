using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Cart.Models.Dto
{

    public class CouponDto
    {        
        public int Id { get; set; }        
        public string Code { get; set; }        
        public double Value { get; set; }
        public int? minAmount { get; set; }
    }
}
