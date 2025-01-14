using AutoMapper;
using MarketPlace.ProductAPI.Models;
using MarketPlace.ProductAPI.Models.Dto;
namespace MarketPlace.ProductAPI.Extensions
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Product, ProductDto>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
