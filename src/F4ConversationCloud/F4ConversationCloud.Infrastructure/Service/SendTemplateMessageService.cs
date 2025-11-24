using F4ConversationCloud.Application.Common.Models.MetaCloudApiModel.Templates;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Configuration;
using F4ConversationCloud.Application.Common.Interfaces.Services;
using F4ConversationCloud.Application.Common.Interfaces.Services.Meta;
using F4ConversationCloud.Application.Common.Models.Templates;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Interfaces.Services.Common;

namespace F4ConversationCloud.Infrastructure.Service
{
    public class SendTemplateMessageService : ISendTemplateMessageService
    {
        private readonly HttpClient _httpClient;
        private IConfiguration _configuration { get; }
        private IAPILogService _logservice { get; }

        public SendTemplateMessageService(HttpClient httpClient, IConfiguration configuration , IAPILogService logService)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logservice = logService;
        }


        public async Task SendTemplateMessageAsync(string userId, string templateName, Dictionary<string, string> parameters)
        {
            try
            {
                var parameterList = parameters
                       .Select(kv => new
                       {
                           type = "text",
                           parameter_name = kv.Key,
                           text = kv.Value
                       })
                    .ToArray();

                var payload = new
                {
                    messaging_product = "whatsapp",
                    to = userId,
                    type = "template",
                    template = new
                    {
                        name = templateName,
                        language = new { code = "en" },
                        components = new[]
                        {
                new
                {
                    type = "body",
                    parameters = parameterList
                }
            }
                    }
                };

                await PostToMetaApi(payload);
            }
            catch (Exception ex)
            {

            }

        }

        public async Task SendTextMessageAsync(string userId, string message)
        {
            try
            {
                var payload = new
                {
                    messaging_product = "whatsapp",
                    to = userId,
                    type = "text",
                    text = new { body = message }
                };

                await PostToMetaApi(payload);
            }
            catch(Exception ex)
            {

            }
        }
        public async Task PostToMetaApi(object payload)
        {
            try
            {
                var FacebookGraphMessageEndpoint = _configuration["WhatsAppAPISettings:FacebookGraphMessageEndpoint"]; // Correct ID
                var accessToken = _configuration["WhatsAppAPISettings:AccessToken"]; // Correct token

                var request = new HttpRequestMessage(HttpMethod.Post, $"{FacebookGraphMessageEndpoint}");

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                request.Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

                var handler = new HttpClientHandler();
                using (var client = new HttpClient(handler, disposeHandler: false))
                {
                    var response = await client.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("Meta API Error: " + errorContent);

                        var payloadJson = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(payload));
                        var to = payloadJson?["to"]?.ToString();

                        if (!string.IsNullOrEmpty(to))
                        {
                            await SendTextMessageAsync(
                                to,
                                ""
                            );
                        }

                        return;
                    }

                    response.EnsureSuccessStatusCode();
                }
            }
            catch (Exception ex) { 
            
            }
        }


        public async Task<dynamic> PostToMetaApi(object requestBody, string PhoneID)
        {
            string apiUrl = string.Empty;
            string methodType = "POST";
            var headers = new Dictionary<string, string>();

            try
            {
                string requestJson = JsonConvert.SerializeObject(requestBody);

                //string token = _configuration["WhatsAppAPISettings:Token"];
                string token = "EAAqZAjK5EFEcBPBe6Lfoyi1pMh3cyrQbaBoyHvmLJeyMaZBnb8LsDPTxfdmAgZBcNZBQJpyOqwlQDMBTiMpmzrzZByRyHorE6U76Cffdf7KPzQZAxSEx7YZCMpZBZAN3wU9X1wTpYkrK0w6ZAHdE8SaKNU26js31LfrYB8dsJuQRF2stqwl26qKhJrLTOBUuTcygZDZD";

                headers = new Dictionary<string, string> { { "Authorization", $"Bearer {token}" } };

                var whatsAppBusinessAccountId = PhoneID;

                var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendTemplateSyncApi.ToString().Replace("{{phone_id}}", whatsAppBusinessAccountId);

                var result = await _logservice.CallExternalAPI<dynamic>(formattedWhatsAppEndpoint,
                                                                    methodType,
                                                                    requestBody,
                                                                    headers,
                                                                    "Send Template",
                                                                    null,
                                                                    true);
                if (result.error != null)
                {
                    return new APIResponse
                    {
                        Status = false,
                        Error = result.error,
                        Message = result.error.error_user_msg

                    };
                }
                else
                {
                    return new APIResponse
                    {
                        Status = true,
                        result = result,
                        Message = "Template Send successFully."
                    };

                }


            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    Message = "Error occured while sending template.",
                    Status = false,
                    Error = ex.Message,
                    StackTrace = ex.StackTrace
                };
            }
        }


    }


}
