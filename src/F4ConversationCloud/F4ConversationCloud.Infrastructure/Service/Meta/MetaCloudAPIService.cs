using F4ConversationCloud.Application.Common.Interfaces.Services.Meta;
using F4ConversationCloud.Application.Common.Models.MetaModel.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System.Net;
using Polly.Extensions.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using F4ConversationCloud.Application.Common.Models.MetaCloudApiModel.Response;
using F4ConversationCloud.Application.Common.Models.MetaCloudApiModel.Exceptions;

namespace F4ConversationCloud.Infrastructure.Service.Meta
{
    public class MetaCloudAPIService: IMetaCloudAPIService
    {


        private readonly HttpClient _httpClient;
        readonly Random jitterer = new Random();
        private WhatsAppBusinessCloudApiConfig _whatsAppConfig;
        public MetaCloudAPIService(WhatsAppBusinessCloudApiConfig whatsAppConfig, string graphAPIVersion = null)
        {
            var retryPolicy = HttpPolicyExtensions.HandleTransientHttpError()
                .WaitAndRetryAsync(1, retryAttempt =>
                {
                    return TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) + TimeSpan.FromMilliseconds(jitterer.Next(0, 100));
                });

            var noOpPolicy = Polly.Policy.NoOpAsync().AsAsyncPolicy<HttpResponseMessage>();

            var services = new ServiceCollection();
            services.AddHttpClient("WhatsAppBusinessApiClient", client =>
            {
                client.BaseAddress = (string.IsNullOrWhiteSpace(graphAPIVersion)) ? WhatsAppBusinessRequestEndpoint.BaseAddress : new Uri(WhatsAppBusinessRequestEndpoint.GraphApiVersionBaseAddress.ToString().Replace("{{api-version}}", graphAPIVersion));
                client.Timeout = TimeSpan.FromMinutes(10);
            }).ConfigurePrimaryHttpMessageHandler(messageHandler =>
            {
                var handler = new HttpClientHandler();

                if (handler.SupportsAutomaticDecompression)
                {
                    handler.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                }

                return handler;
            }).AddPolicyHandler(request => request.Method.Equals(HttpMethod.Get) ? retryPolicy : noOpPolicy);

            var serviceProvider = services.BuildServiceProvider();

            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();

            _httpClient = httpClientFactory.CreateClient("WhatsAppBusinessApiClient");
            _whatsAppConfig = whatsAppConfig;
        }


        public MetaCloudAPIService(HttpClient httpClient, WhatsAppBusinessCloudApiConfig whatsAppConfig)
        {
            _httpClient = httpClient;
            _whatsAppConfig = whatsAppConfig;
        }

        public void SetWhatsAppBusinessConfig(WhatsAppBusinessCloudApiConfig cloudApiConfig)
        {
            _whatsAppConfig = cloudApiConfig;
        }
        public async Task<TemplateMessageCreationResponse> CreateTemplateMessageAsync(string whatsAppBusinessAccountId, object template, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.CreateTemplateMessage.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);
            return await WhatsAppBusinessPostAsync<TemplateMessageCreationResponse>(template, formattedWhatsAppEndpoint, cancellationToken);
        }


        private async Task<T> WhatsAppBusinessPostAsync<T>(object whatsAppDto, string whatsAppBusinessEndPoint, CancellationToken cancellationToken = default) where T : new() { 
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _whatsAppConfig.AccessToken);

            T result = new T();
            string jsonString = JsonSerializer.Serialize(whatsAppDto);
            var content = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");
            cancellationToken.ThrowIfCancellationRequested();

            var response = await _httpClient.PostAsync(whatsAppBusinessEndPoint, content, cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {

                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    result = await JsonSerializer.DeserializeAsync<T>(stream, cancellationToken: cancellationToken);
                }

            }
            else
            {
                WhatsAppErrorResponse whatsAppErrorResponse = new WhatsAppErrorResponse();

                using (var stream = await response.Content.ReadAsStreamAsync(cancellationToken))
                {
                    whatsAppErrorResponse = await JsonSerializer.DeserializeAsync<WhatsAppErrorResponse>(stream, cancellationToken: cancellationToken);
                }
                throw new WhatsappBusinessCloudAPIException(new HttpRequestException(whatsAppErrorResponse.Error.Message), response.StatusCode, whatsAppErrorResponse);
            }
            return result;



        }







    }
}
