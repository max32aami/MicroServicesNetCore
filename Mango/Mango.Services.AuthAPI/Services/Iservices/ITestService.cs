using Mango.Services.AuthAPI.Models.DTO;

namespace Mango.Services.AuthAPI.Services.Iservices
{
    public interface ITestService
    {
        Task<UserDto> Register(RegistrationRequestsDto registrationRequestsDto);
        Task<LoginRequestDto> login(LoginRequestDto loginRequestDto);
    }
}
