using Mango.Services.AuthAPI.Models.DTO;

namespace Mango.Services.AuthAPI.Services.Iservices
{
    public interface IAuthService
    {
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        Task<string> Register(RegistrationRequestsDto registrationRequestsDto);
        Task<bool> AssignRole(string email, string roleName);
    }
}
