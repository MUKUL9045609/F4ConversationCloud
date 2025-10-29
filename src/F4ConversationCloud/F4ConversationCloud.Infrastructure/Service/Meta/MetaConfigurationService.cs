
using F4ConversationCloud.Application.Common.Interfaces.Services.Meta;

using F4ConversationCloud.Application.Common.Models.MetaModel.Configurations;

using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System.Net;


namespace F4ConversationCloud.Infrastructure.Service.Meta
{
    public class MetaConfigurationService 
    {
        private readonly IServiceCollection _services;
        public MetaConfigurationService(IServiceCollection services)
        {
            _services = services;
        }

        public static void AddWhatsAppBusinessCloudApiService(IServiceCollection services, WhatsAppBusinessCloudApiConfig whatsAppConfig, string graphAPIVersion = null)
        {
            Random jitterer = new Random();

            var retryPolicy = HttpPolicyExtensions.HandleTransientHttpError()
                .WaitAndRetryAsync(1, retryAttempt =>
                {
                    return TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) + TimeSpan.FromMilliseconds(jitterer.Next(0, 100));
                });

            var noOpPolicy = Policy.NoOpAsync().AsAsyncPolicy<HttpResponseMessage>();

            //services.AddTransient<IWhatsAppBusinessClientFactory, WhatsAppBusinessClientFactory>();

            services.AddSingleton(new WhatsAppBusinessCloudApiConfig
            {
                WhatsAppBusinessPhoneNumberId = whatsAppConfig.WhatsAppBusinessPhoneNumberId,
                WhatsAppBusinessAccountId = whatsAppConfig.WhatsAppBusinessAccountId,
                WhatsAppBusinessId = whatsAppConfig.WhatsAppBusinessId,
                AccessToken = whatsAppConfig.AccessToken,
                AppName = whatsAppConfig.AppName,
                Version = whatsAppConfig.Version
            });
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