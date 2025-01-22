using MarketPlace.OrderAPI.Models.Dto;

namespace MarketPlace.OrderAPI.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAll();
    }
}
