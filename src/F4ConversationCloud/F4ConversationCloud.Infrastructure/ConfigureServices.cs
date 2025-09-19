using F4ConversationCloud.Application.Common.Interfaces.Repositories.Client;
using F4ConversationCloud.Application.Common.Interfaces.Repositories.Onboarding;
using F4ConversationCloud.Application.Common.Interfaces.Repositories.SuperAdmin;
using F4ConversationCloud.Application.Common.Interfaces.Services;
using F4ConversationCloud.Application.Common.Interfaces.Services.Client;
using F4ConversationCloud.Application.Common.Interfaces.Services.Onboarding;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Services;
using F4ConversationCloud.Infrastructure.Interfaces;
using F4ConversationCloud.Infrastructure.Persistence;
using F4ConversationCloud.Infrastructure.Repositories.Client;
using F4ConversationCloud.Infrastructure.Repositories.Onboarding;
using F4ConversationCloud.Infrastructure.Repositories.SuperAdmin;
using F4ConversationCloud.Infrastructure.Service;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using F4ConversationCloud.Infrastructure.Service.SuperAdmin;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using F4ConversationCloud.Application.Common.Interfaces.Services.Meta;
using F4ConversationCloud.Infrastructure.Service.MetaServices;


namespace F4ConversationCloud.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<DbContext>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<IMessageService, MessageService>();

            services.AddScoped<IAPILogService, APILogService>();

            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddHttpContextAccessor(); 
            services.AddScoped<IFileUploadService, FileUploadService>();
            services.AddScoped<IEmailSenderService, EmailSenderService>();

            services.AddScoped<ICampaignRepository, CampaignRepository>();
            services.AddScoped<ICampaignService, CampaignService>();
            services.AddScoped<ISuperAdminAuthService, SuperAdminAuthService>();
            services.AddScoped<ISuperAdminAuthRepository, SuperAdminAuthRepository>();
            services.AddScoped<IOnboardingService, OnboardingService>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IWhatsAppCloudeService, WhatsAppCloudeService>();
            return services;
        }
    }
}
