using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GymnArteApp.Server.Services.Interface;
using Microsoft.IdentityModel.Tokens;

namespace GymnArteApp.Server.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(Models.User user, Models.Enums.UserRole role)
        {
            var jwtSection  = _config.GetSection("Jwt");
            var key         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!));
            var expires     = int.TryParse(jwtSection["ExpiresInMinutes"], out var m) ? m : 480;

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub,   user.UserId.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(JwtRegisteredClaimNames.Jti,   Guid.NewGuid().ToString()),
                new(ClaimTypes.Name,               $"{user.Name} {user.Surname}"),
                new(ClaimTypes.Role,               role.ToString()),
                new("userId",                      user.UserId.ToString()),
                new("userName",                    user.UserName),
            };

            var token = new JwtSecurityToken(
                issuer:            jwtSection["Issuer"],
                audience:          jwtSection["Audience"],
                claims:            claims,
                expires:           DateTime.UtcNow.AddMinutes(expires),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
