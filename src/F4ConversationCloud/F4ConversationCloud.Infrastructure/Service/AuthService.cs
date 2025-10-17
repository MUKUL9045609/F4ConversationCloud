using F4ConversationCloud.Application.Common.Interfaces.Repositories.Onboarding;
using F4ConversationCloud.Application.Common.Interfaces.Services;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Domain.Entities.SuperAdmin;
using F4ConversationCloud.Domain.Extension;
using F4ConversationCloud.Domain.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace F4ConversationCloud.Infrastructure.Service
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthRepository _authRepository;

        public AuthService(IConfiguration configuration, IAuthRepository authRepository)
        {
            _configuration = configuration;
            _authRepository = authRepository;
        }

        public async Task<Tuple<string, int>> GenerateToken(UserContextModel userContextModel)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.NameIdentifier, userContextModel.UserId.ToString()),
            new Claim(ClaimTypes.Name, userContextModel.Name),
            new Claim(ClaimTypes.Role, userContextModel.Role),
            new Claim("IsCreateTemplate", userContextModel.IsCreateTemplate.ToString()),
            new Claim("IsDeleteTemplate", userContextModel.IsDeleteTemplate.ToString()),
            new Claim("IsEditTemplate", userContextModel.IsEditTemplate.ToString()),
            new Claim("IsView", userContextModel.IsView.ToString())
            });

            var token = new JwtSecurityToken(_configuration["JWT:Issuer"],
              _configuration["JWT:Audience"],
              claims: claimsIdentity.Claims,
              expires: DateTime.UtcNow.AddSeconds(Convert.ToInt32(_configuration["JWT:expirationTimeInSeconds"])),
              signingCredentials: credentials);

            return Tuple.Create(new JwtSecurityTokenHandler().WriteToken(token), Convert.ToInt32(_configuration["JWT:expirationTimeInSeconds"]));
        }

        public async Task<APILoginResponse> ValidateUser(UserDetailsDTO request)
        {
            try
            {
                var userDetails = _authRepository.GetClientInfoByEmailId(request).Result;

                if (userDetails != null)
                {

                    if (PasswordHasherHelper.VerifyPassword(request.Password, userDetails.Password))
                    {
                        var token =  GenerateToken(new UserContextModel()
                        {
                            UserId = userDetails.Id,
                            Name = userDetails.FirstName,
                            Role = userDetails.Role ?? string.Empty,
                            IsCreateTemplate = false,
                            IsDeleteTemplate = true,
                            IsEditTemplate = false,
                            IsView = false
                        }).Result;

                        return new APILoginResponse()
                        {
                            Status = true,
                            Message = "Success",
                            Data = new APILoginData
                            {
                                Token = token.Item1,
                                ExpiryInSeconds = token.Item2
                            }
                        };
                    }
                    else
                    {
                        throw new UnauthorizedAccessException("Invalid Credentails");

                    }

                }
                else
                {
                    throw new UnauthorizedAccessException("Invalid Credentails");

                }
            }
            catch (Exception ex)
            {
                throw new UnauthorizedAccessException("Invalid Credentails");

            }

        }
    }
}
