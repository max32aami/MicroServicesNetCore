using Mango.Services.AuthAPI.Models.DTO;
using Mango.Services.AuthAPI.Services.Iservices;

namespace Mango.Services.AuthAPI.Services
{
    public class TestService : ITestService
    {
        public Task<LoginRequestDto> login(LoginRequestDto loginRequestDto)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> Register(RegistrationRequestsDto registrationRequestsDto)
        {
            throw new NotImplementedException();
        }
    }
}
