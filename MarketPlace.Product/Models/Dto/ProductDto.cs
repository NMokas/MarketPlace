namespace MarketPlace.ProductAPI.Models.Dto
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string? Description { get; set; }
        public int Discount { get; set; } = 0;
        public CategoryDto? Category { get; set; }
        public int CategoryId { get; set; }
    }
}
