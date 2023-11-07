using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EasyControl.Api.Domain.Models;
using Microsoft.IdentityModel.Tokens;

namespace EasyControl.Api.Domain.Services.Classes
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = _configuration["KeySecret"];

            if (key == null)
            {
                throw new ArgumentNullException(
                    nameof(key),
                    "KeySecret configuration value is null."
                );
            }

            var keyBytes = Encoding.ASCII.GetBytes(key);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new(ClaimTypes.Email, user.Email)
                    }
                ),
                Expires = DateTime.UtcNow.AddHours(
                    Convert.ToInt32(_configuration["TokenValidityHours"])
                ),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(keyBytes),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
