using Azure;
using Mango.Services.AuthAPI.Models.DTO;
using Mango.Services.AuthAPI.Services.Iservices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        public readonly IAuthService _authService;
        protected ResponseDto _response;
        public AuthAPIController(IAuthService authService)
        {
            _authService = authService;
            _response = new();
        }   

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestsDto model)
        {
            var result = await _authService.Register(model);
            if(!string.IsNullOrEmpty(result))
            {
                _response.IsSucess = false;
                _response.Message = result;
                return BadRequest(_response);
            }
            else
            {
                return Ok(_response);
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var loginResponse = await _authService.Login(model);
            if(loginResponse.User == null) 
            {
               _response.IsSucess = false;
                _response.Message = "Wrong Credntials";
                return BadRequest(_response);
            }
            else
            {
                _response.Result = loginResponse;
                _response.IsSucess = true;
                _response.Message = "Login Successful";
                return Ok(_response);
            }
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestsDto model)
        {
            var loginResponse = await _authService.AssignRole(model.Email,model.Role.ToUpper());
            if (!loginResponse)
            {
                _response.IsSucess = false;
                _response.Message = "Error Encountered, Wrong data";
                return BadRequest(_response);
            }
            else
            {
                _response.Result = loginResponse;
                _response.IsSucess = true;
                _response.Message = "Role Added Successfully";
                return Ok(_response);
            }
        }

    }
}
