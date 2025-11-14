using F4ConversationCloud.Application.Common.Interfaces.Services.Client;
using F4ConversationCloud.Infrastructure.Service.Client;
using Microsoft.AspNetCore.Mvc;

namespace F4ConversationCloud.ClientAdmin
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddWebUIServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();

            services.AddAuthorization();
            services.AddSingleton<ICurrentUserService, CurrentUserService>();

            services.Configure<ApiBehaviorOptions>(options =>
                options.SuppressModelStateInvalidFilter = true);

            return services;
        }
    }
}
