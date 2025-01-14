using System.ComponentModel.DataAnnotations;

namespace MarketPlace.CategoryAPI.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name{ get; set; }
        public string? Comment { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime? UpdateDate { get; set; } = null;

    }
}
