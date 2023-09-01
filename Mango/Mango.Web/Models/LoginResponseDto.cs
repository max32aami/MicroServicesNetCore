using Mango.Web.Models;
using Microsoft.Extensions.Primitives;

namespace Mango.Web.Models
{
    public class LoginResponseDto
    {
        public UserDto User { get; set; }
        public string Token { get; set; }
    }
}
