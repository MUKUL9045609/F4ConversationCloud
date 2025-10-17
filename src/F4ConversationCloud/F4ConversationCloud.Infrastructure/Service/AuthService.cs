using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Infrastructure.Service
{
    public class AuthService
    {
        private readonly IConfiguration _configuration;
        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Tuple<string, int>> GenerateToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim(ClaimTypes.Name, "Admin"),
            new Claim(ClaimTypes.Role, "Admin"),
            });

            var token = new JwtSecurityToken(_configuration["JWT:Issuer"],
              _configuration["JWT:Audience"],
              claims: claimsIdentity.Claims,
              expires: DateTime.UtcNow.AddSeconds(Convert.ToInt32(_configuration["JWT:expirationTimeInSeconds"])),
              signingCredentials: credentials);

            return Tuple.Create(new JwtSecurityTokenHandler().WriteToken(token), Convert.ToInt32(_configuration["JWT:expirationTimeInSeconds"]));
        }
    }
}
