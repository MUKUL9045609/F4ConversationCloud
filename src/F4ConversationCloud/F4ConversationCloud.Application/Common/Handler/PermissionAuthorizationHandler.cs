using BuldanaUrban.Domain.Helpers;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Handler
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PermissionAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {

            HttpContext? httpContext = _httpContextAccessor.HttpContext;

            if (httpContext != null)
            {
                if (context.User.HasClaim(c => c.Type == TemplateAccess.CreateTemplate.GetDisplayName() && c.Value == requirement.Permission) && httpContext.GetRouteValue("action")?.ToString() == TemplateAPIEndPoints.CreateTemplate.GetDisplayName())
                {
                    context.Succeed(requirement);
                }
                else if (context.User.HasClaim(c => c.Type == TemplateAccess.DeleteTemplate.GetDisplayName() && c.Value == requirement.Permission) && httpContext.GetRouteValue("action")?.ToString() == TemplateAPIEndPoints.DeleteTemplate.GetDisplayName())
                {
                    context.Succeed(requirement);
                }
                else if (context.User.HasClaim(c => c.Type == TemplateAccess.EditTemplate.GetDisplayName() && c.Value == requirement.Permission) && httpContext.GetRouteValue("action")?.ToString() == TemplateAPIEndPoints.EditTemplate.GetDisplayName())
                {
                    context.Succeed(requirement);
                }
                else if (context.User.HasClaim(c => c.Type == TemplateAccess.ViewTemplate.GetDisplayName() && c.Value == requirement.Permission) && httpContext.GetRouteValue("action")?.ToString() == TemplateAPIEndPoints.ViewTemplate.GetDisplayName())
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }
}
