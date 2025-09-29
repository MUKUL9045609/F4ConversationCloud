using F4ConversationCloud.Application.Common.Interfaces.Repositories.Client;
using F4ConversationCloud.Application.Common.Interfaces.Repositories.Onboarding;
using F4ConversationCloud.Application.Common.Interfaces.Repositories.SuperAdmin;
using F4ConversationCloud.Application.Common.Interfaces.Services;
using F4ConversationCloud.Application.Common.Interfaces.Services.Client;
using F4ConversationCloud.Application.Common.Interfaces.Services.Meta;
using F4ConversationCloud.Application.Common.Interfaces.Services.Onboarding;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models.MetaModel.Configurations;
using F4ConversationCloud.Application.Common.Services;
using F4ConversationCloud.Infrastructure.Interfaces;
using F4ConversationCloud.Infrastructure.Persistence;
using F4ConversationCloud.Infrastructure.Repositories.Client;
using F4ConversationCloud.Infrastructure.Repositories.Onboarding;
using F4ConversationCloud.Infrastructure.Repositories.SuperAdmin;
using F4ConversationCloud.Infrastructure.Service;
using F4ConversationCloud.Infrastructure.Service.Meta;
using F4ConversationCloud.Infrastructure.Service.MetaServices;
using F4ConversationCloud.Infrastructure.Service.SuperAdmin;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System.Net;


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


            //Super Admin Services and Repositories
            services.AddScoped<ISuperAdminAuthService, SuperAdminAuthService>();
            services.AddScoped<ISuperAdminAuthRepository, SuperAdminAuthRepository>();
            //services.AddScoped<IOnboardingService, OnboardingService>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<HttpClient>();
            //services.AddScoped<IWhatsAppCloudeService, WhatsAppCloudeService>();
            services.AddScoped<IUserManagementService, UserManagementService>();
            services.AddScoped<IUserManagementRepository, UserManagementRepository>();
            services.AddScoped<IClientUserManagementService, ClientUserManagementService>();
            services.AddScoped<IClientUserManagementRepository, ClientUserManagementRepository>();
            services.AddScoped<IClientManagementService, ClientManagementService>();
            services.AddScoped<IClientManagementRepository, ClientManagementRepository>();
            services.AddScoped<IWebhookService, WebhookService>();
            services.AddScoped<ITemplateManagementService, TemplateManagementService>();
           // services.AddScoped<IMetaCloudAPIService , MetaCloudAPIService>();
            services.AddScoped<IF4AppCloudeService, F4AppCloudeService>();
            return services;
        }


        public static void AddWhatsAppBusinessCloudApiService(this IServiceCollection services, WhatsAppBusinessCloudApiConfig whatsAppConfig, string graphAPIVersion = null)
        {
            Random jitterer = new Random();

            var retryPolicy = HttpPolicyExtensions.HandleTransientHttpError()
                .WaitAndRetryAsync(1, retryAttempt =>
                {
                    return TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) + TimeSpan.FromMilliseconds(jitterer.Next(0, 100));
                });

            var noOpPolicy = Policy.NoOpAsync().AsAsyncPolicy<HttpResponseMessage>();

          //  services.AddTransient<IWhatsAppBusinessClientFactory, WhatsAppBusinessClientFactory>();

            //services.AddSingleton(new WhatsAppBusinessCloudApiConfig
            //{
            //    WhatsAppBusinessPhoneNumberId = whatsAppConfig.WhatsAppBusinessPhoneNumberId,
            //    WhatsAppBusinessAccountId = whatsAppConfig.WhatsAppBusinessAccountId,
            //    WhatsAppBusinessId = whatsAppConfig.WhatsAppBusinessId,
            //    AccessToken = whatsAppConfig.AccessToken,
            //    AppName = whatsAppConfig.AppName,
            //    Version = whatsAppConfig.Version
            //});
            // services.AddScoped<IWhatsAppTemplateManagementInterface, WhatsAppTemplateManagementInterface>();
            services.AddHttpClient<IMetaCloudAPIService, MetaCloudAPIService>(options =>
            {
                options.BaseAddress = (string.IsNullOrWhiteSpace(graphAPIVersion)) ? WhatsAppBusinessRequestEndpoint.BaseAddress : new Uri(WhatsAppBusinessRequestEndpoint.GraphApiVersionBaseAddress.ToString().Replace("{{api-version}}", graphAPIVersion));
                options.Timeout = TimeSpan.FromMinutes(10);
            }).ConfigurePrimaryHttpMessageHandler(messageHandler =>
            {
                var handler = new HttpClientHandler();

                if (handler.SupportsAutomaticDecompression)
                {
                    handler.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                }

                return handler;
            }).AddPolicyHandler(request => request.Method.Equals(HttpMethod.Get) ? retryPolicy : noOpPolicy);

        }

    }
}
