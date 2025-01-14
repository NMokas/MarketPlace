using MarketPlace.AuthAPI.Models.Dto;
using MarketPlace.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        protected ResponseDto response;
        public AuthAPIController(IAuthService authService)
        {
            _authService = authService;
            response=new ResponseDto();
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegistrationRequestDto model)
        {
            var errorMessage=await _authService.Register(model);
           
            if (!string.IsNullOrEmpty(errorMessage))
            {
                response.IsSuccess = false;
                response.Message = errorMessage;
                return BadRequest(response);
            }
            var assignRoleSuccessful = await _authService.AssignRole(model.Role, model.Email);

            if (!assignRoleSuccessful)
            {
                response.IsSuccess = false;
                response.Message = "Error encountred";
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {

            var loginResponse = await _authService.Login(model);

            if (loginResponse.User == null && loginResponse.Token == "")
            {
                response.IsSuccess = false;
                response.Message = "Login Invalid";
                return BadRequest(response);
            }
            Response.Cookies.Append("jwt", loginResponse.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(30)
            });
            response.Result = loginResponse;
            response.IsSuccess = true;
            response.Message = "Login Valid";

            return Ok(response);

        }
        [HttpPut("ApproveBrand")]
        public async Task<IActionResult> ApproveBrand(string email)
        {

            var Approved = await _authService.ActivateAcc(email);

            if (!Approved)
            {
                response.IsSuccess = false;
                response.Message = "Login Invalid";
                return BadRequest(response);
            }
            response.Result = Approved;
            response.IsSuccess = true;
            response.Message = "Login Valid";

            return Ok(response);

        }
        //[Authorize]
        //[HttpPost("AssignRole")]
        //public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto model)
        //{

        //    var assignRoleSuccessful = await _authService.AssignRole(model.Role,model.Email);

        //    if (!assignRoleSuccessful)
        //    {
        //        response.IsSuccess = false;
        //        response.Message = "Error encountred";
        //        return BadRequest(response);
        //    }
        //    response.Result = assignRoleSuccessful;
        //    response.IsSuccess = true;
        //    response.Message = "Login Valid";

        //    return Ok(response);

        //}


    }
}
