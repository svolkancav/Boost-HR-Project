using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Common
{
    public class TokenService : ITokenService
    {
        private readonly string _secretKey = "EasyHR123456789"; // Özel bir anahtar kullanın

        public async Task<string> GenerateTokenAsync(int companyId)
        {
            return await Task.Run(() => GenerateToken(companyId));
        }

        public async Task<bool> ValidateTokenAsync(string token, int companyId)
        {
            return await Task.Run(() => ValidateToken(token, companyId));
        }

        private string GenerateToken(int companyId)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, companyId.ToString())
            }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private bool ValidateToken(string token, int companyId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            SecurityToken validatedToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);

            var userIdClaim = principal.FindFirst(ClaimTypes.Name)?.Value;
            if (userIdClaim == companyId.ToString())
            {
                return true;
            }

            return false;
        }
    }
}
