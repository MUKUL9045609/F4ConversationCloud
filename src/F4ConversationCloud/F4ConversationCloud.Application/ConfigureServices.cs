using F4ConversationCloud.Application.Common.Interfaces.IWebServices;
using F4ConversationCloud.Application.Common.Interfaces.Services;
using F4ConversationCloud.Application.Common.Services;
using Microsoft.Extensions.DependencyInjection;

namespace F4ConversationCloud.Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ICampaignService, CampaignService>();
            services.AddScoped<IOnboardingService, OnboardingService>();
            return services;
        }
    }
}
