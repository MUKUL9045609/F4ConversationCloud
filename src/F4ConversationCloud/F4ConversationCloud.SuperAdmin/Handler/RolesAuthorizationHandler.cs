using F4ConversationCloud.Application.Common.Interfaces.Services.Client;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Security.Claims;

namespace F4ConversationCloud.SuperAdmin.Handler
{
    public class RolesAuthorizationHandler : AuthorizationHandler<RolesAuthorizationRequirement>, IAuthorizationHandler
    {
        private readonly ISuperAdminAuthService _superAdminAuthService;
        private readonly ICurrentUserService _currentUserService;
        public RolesAuthorizationHandler(ISuperAdminAuthService superAdminAuthService,ICurrentUserService currentUserService)
        {
            _superAdminAuthService = superAdminAuthService;
            _currentUserService = currentUserService;
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
                var roles = requirement.AllowedRoles; 
                var email = _currentUserService.CurrentUserEmail;

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
