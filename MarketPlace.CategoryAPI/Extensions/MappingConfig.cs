using AutoMapper;
using MarketPlace.CategoryAPI.Models;
using MarketPlace.CategoryAPI.Models.Dto;
namespace MarketPlace.CategoryAPI.Extensions
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Category, CategoryDto>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
