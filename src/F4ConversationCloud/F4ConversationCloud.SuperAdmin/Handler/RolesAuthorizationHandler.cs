using F4ConversationCloud.Application.Common.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Security.Claims;

namespace F4ConversationCloud.SuperAdmin.Handler
{
    public class RolesAuthorizationHandler : AuthorizationHandler<RolesAuthorizationRequirement>, IAuthorizationHandler
    {
        private readonly ISuperAdminAuthService _superAdminAuthService;
        public RolesAuthorizationHandler(ISuperAdminAuthService superAdminAuthService)
        {
            _superAdminAuthService = superAdminAuthService;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       RolesAuthorizationRequirement requirement)
        {
            if (context.User == null || !context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            var validRole = false;
            if (requirement.AllowedRoles == null || requirement.AllowedRoles.Any() == false)
            {
                validRole = true;
            }
            else
            {
                var claims = context.User.Claims;
                var roles = requirement.AllowedRoles;
                var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;

                validRole = _superAdminAuthService.CheckValidRole(roles, email).Result;
            }

            if (validRole)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
