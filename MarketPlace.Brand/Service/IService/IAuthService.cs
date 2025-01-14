using MarketPlace.AuthAPI.Models.Dto;

namespace MarketPlace.AuthAPI.Service.IService
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationRequestDto registrationRequestDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        Task<bool> AssignRole(string roleName,string email);
        Task<bool> ActivateAcc(string email);
    }
}
