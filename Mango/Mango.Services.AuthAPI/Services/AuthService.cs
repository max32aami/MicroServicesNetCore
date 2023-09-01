using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.DTO;
using Mango.Services.AuthAPI.Services.Iservices;
using Microsoft.AspNetCore.Identity;

namespace Mango.Services.AuthAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(AppDbContext db, IJwtTokenGenerator jwtTokenGenerator , UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(p => p.UserName.ToLower() == email.ToLower());
            if (user != null) 
            {
                if(!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(p => p.UserName.ToLower() == loginRequestDto.UserName.ToLower());
            bool PasswordCHeck = await _userManager.CheckPasswordAsync(user,loginRequestDto.Password);
            if (!PasswordCHeck || user ==null) 
            {
                return new LoginResponseDto()
                {
                    User = null,
                    Token = ""
                };
            }
            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenGenerator.GenerateToken(user,roles);

            UserDto userDto = new()
            {
                Email = user.Email,
                ID = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber
            };
            LoginResponseDto responseDto = new()
            {
                User = userDto,
                Token = token

            };
            return responseDto;
        }

        public async Task<string> Register(RegistrationRequestsDto registrationRequestsDto)
        {
            ApplicationUser user = new()
            {
                UserName = registrationRequestsDto.Email,
                Email = registrationRequestsDto.Email,
                NormalizedEmail = registrationRequestsDto.Email.ToUpper(),
                Name = registrationRequestsDto.Name,
                PhoneNumber = registrationRequestsDto.PhoneNumber
            };
            
            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestsDto.Password);
                if(result.Succeeded)
                {
                    var userToReturn = _db.ApplicationUsers.FirstOrDefault(p => p.UserName==registrationRequestsDto.Email);
                    UserDto userDto = new()
                    {
                        Name = userToReturn.Name,
                        ID = userToReturn.Id,
                        PhoneNumber = userToReturn.PhoneNumber,
                        Email = userToReturn.Email
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
                return "Error Encountered";
            }

        }
    }
}
