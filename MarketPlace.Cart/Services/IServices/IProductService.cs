using MarketPlace.Cart.Models.Dto;

namespace MarketPlace.Cart.Services.IServices
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
    }
}
