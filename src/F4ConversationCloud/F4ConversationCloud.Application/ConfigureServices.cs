using F4ConversationCloud.Application.Common.Interfaces.IWebServices;
using F4ConversationCloud.Application.Common.Interfaces.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace F4ConversationCloud.Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            return services;
        }
    }
}
