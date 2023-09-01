using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.CompilerServices;
using System.Text;

namespace Mango.Services.ProductAPI.Extensions
{
    public static class WebApplicationBuilderExtensionsNew
    {
        public static WebApplicationBuilder AddAppAuthentications(this WebApplicationBuilder builder)
        {
            var sectionSettings = builder.Configuration.GetSection("APISettings");
            var secret = sectionSettings.GetValue<string>("Secret");
            var issuer = sectionSettings.GetValue<string>("Issuer");
            var audience = sectionSettings.GetValue<string>("Audience");

            var key = Encoding.ASCII.GetBytes(secret);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    ValidateAudience = true,
                };
            });
            return builder;

        }
    }
}
