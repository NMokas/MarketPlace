using System.ComponentModel.DataAnnotations;

namespace MarketPlace.CouponAPI.Models
{
    public class Coupon
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public double Value { get; set; }
        public int? minAmount { get; set; }
        public DateTime CreationDate { get; set; }=DateTime.Now;
        public DateTime ExpirationDate { get; set; } = DateTime.Now.AddDays(30);
        public DateTime? UsageDate { get; set; } = null;
    }
}
