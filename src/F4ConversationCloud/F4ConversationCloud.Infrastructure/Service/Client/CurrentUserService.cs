using F4ConversationCloud.Application.Common.Interfaces.Services.Client;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Infrastructure.Service.Client
{
    public class CurrentUserService:ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        public string? Name => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
        public string? Role => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);
        public string? BusinessId => _httpContextAccessor.HttpContext?.User?.FindFirstValue("BusinessId");
        public string? ClientInfoId => _httpContextAccessor.HttpContext?.User?.FindFirstValue("ClientInfoId");

        public string? CurrentUserEmail => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);
        public string? CurrentUserPhoneNumber => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.MobilePhone);
    }
}
