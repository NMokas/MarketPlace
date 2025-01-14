using MarketPlace.ProductAPI.Models.Dto;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketPlace.ProductAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Price{ get; set; }
        public string? Description{ get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedTime { get; set; } = null;
        [Range(0,100)]
        public int Discount { get; set; } = 0;
        public DateTime? DiscountBeginDate { get; set; } = null;
        public DateTime? DiscountEndDate { get; set; } = null;
        public CategoryDto? Category{ get; set; }
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public string? ImgUrl { get; set; } = null;
    }
}
