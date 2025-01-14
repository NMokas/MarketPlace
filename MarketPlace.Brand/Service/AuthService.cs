using MarketPlace.AuthAPI.Data;
using MarketPlace.AuthAPI.Models;
using MarketPlace.AuthAPI.Models.Dto;
using MarketPlace.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;

namespace MarketPlace.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        public AuthService(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager,ApplicationDbContext db, IJwtTokenGenerator jwtTokenGenerator)
        {
            _db=db;
            _userManager=userManager;
            _roleManager=roleManager;
            _jwtTokenGenerator=jwtTokenGenerator;
        }

        public async Task<bool> ActivateAcc(string email)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
            if (user.IsBrand || user != null)
            {
                user.IsApproved=true;
                _db.ApplicationUsers.Update(user);
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> AssignRole(string roleName, string email)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
            if (user != null)
            {
                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                }
                if (roleName == "Brand")
                {
                    user.IsBrand = true;
                }
                _db.ApplicationUsers.Update(user);
                _db.SaveChanges();
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            return false;
        }



        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if (user == null || isValid == false)
            {
                return new LoginResponseDto() { User = null, Token = "" };
            }

            ////Generate the JWT token
            var roles = await _userManager.GetRolesAsync(user);
            var tokenString = _jwtTokenGenerator.GenerateToken(user);


            UserDto userDto = new()
            {
                Email = user.Email,
                ID = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber
            };
            LoginResponseDto loginResponseDto = new LoginResponseDto()
            {
                User = userDto,
                Token = tokenString
            };
            return loginResponseDto;
            }

            public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            ApplicationUser user = new()
            {
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                NormalizedEmail = registrationRequestDto.Email.ToUpper(),
                Name = registrationRequestDto.Name,
                PhoneNumber = registrationRequestDto.PhoneNumber,

            };

            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);
                if (result.Succeeded)
                {
                    var userToReturn = _db.ApplicationUsers.First(u => u.UserName == registrationRequestDto.Email);

                    UserDto userDto = new()
                    {
                        Email=userToReturn.Email,
                        ID=userToReturn.Id,
                        Name=userToReturn.Name,
                        PhoneNumber=userToReturn.PhoneNumber
                    };

                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch(Exception ex) 
            {
                    return "Error Encontred";

            }

        }
    }
}
