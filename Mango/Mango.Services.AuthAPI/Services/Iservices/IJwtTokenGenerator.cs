using Mango.Services.AuthAPI.Models;

namespace Mango.Services.AuthAPI.Services.Iservices
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> Roles);
    }
}
