using MarketPlace.AuthAPI.Models;

namespace MarketPlace.AuthAPI.Service.IService
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser applicationUser);
        
    }
}
