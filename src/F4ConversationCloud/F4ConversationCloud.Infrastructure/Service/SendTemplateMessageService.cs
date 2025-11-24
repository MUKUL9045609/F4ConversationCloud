using F4ConversationCloud.Application.Common.Models.MetaCloudApiModel.Templates;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using F4ConversationCloud.Application.Common.Interfaces.Services;
using F4ConversationCloud.Application.Common.Interfaces.Services.Meta;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.Templates;

namespace F4ConversationCloud.Infrastructure.Service
{
    public class SendTemplateMessageService : ISendTemplateMessageService
    {
        private readonly HttpClient _httpClient;
        private IConfiguration _configuration { get; }
        private IAPILogService _logservice { get; }

        public SendTemplateMessageService(HttpClient httpClient, IConfiguration configuration, IAPILogService logService)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logservice = logService;
        }

        public async Task SendTemplateMessageAsync(SendTemplateDTO sendTemplateDTO)
        {
            try
            {
                var parameterList = sendTemplateDTO.TemplateValues
                    .Select(kv => new
                    {
                        type = "text",
                        //parameter_name = kv.Key,
                        text = kv.Value
                    })
                    .ToArray();

                var payload = new
                {
                    messaging_product = "whatsapp",
                    to = sendTemplateDTO.SendTo,
                    type = "template",
                    template = new
                    {
                        name = sendTemplateDTO.TemplateName,
                        language = new { code = "en" },
                        components = new object[]
                        {
                            new
                            {
                                type = "header",
                                parameters = new object[]
                                {
                                    new
                                    {
                                        type = "image",
                                        image = new
                                        {
                                            link = sendTemplateDTO.TemplateUrl
                                        }
                                    }
                                }
                            }
                            
                            //,
                            //new
                            //{
                            //    type = "body",
                            //    parameters = parameterList
                            //}
                        }
                    }
                };
                

                await MetaSendMessageTemplateAPICall(payload, sendTemplateDTO.PhoneId);
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

                await MetaSendMessageTemplateAPICall(payload);
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<dynamic> MetaSendMessageTemplateAPICall(object requestBody, string PhoneID = null)
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

        public Dictionary<string, string> BuildParameterDictionaryFromObject(List<string> parameterKeys, Object Details)
        {
            try
            {
                var parameterDict = new Dictionary<string, string>();

                if (Details == null)
                    return parameterDict;

                var type = Details.GetType();

                foreach (var key in parameterKeys)
                {
                    var prop = type.GetProperty(key,
                        System.Reflection.BindingFlags.IgnoreCase |
                        System.Reflection.BindingFlags.Public |
                        System.Reflection.BindingFlags.Instance);

                    if (prop != null)
                    {
                        var value = prop.GetValue(Details)?.ToString() ?? string.Empty;
                        parameterDict[key.ToLower()] = value;  // always use lower keys in dict
                    }
                    else
                    {
                        parameterDict[key.ToLower()] = string.Empty; // fallback if not found
                    }
                }

                return parameterDict;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task SendOfferCarouselAsync(string userId, string TemplateName)
        {
            var payload = new
            {
                messaging_product = "whatsapp",
                to = userId,
                type = "template",
                template = new
                {
                    name = TemplateName,
                    language = new { code = "en" },
                    components = new object[]
                    {
                    new
                    {
                        type = "carousel",
                        cards = new object[]
                        {
                            new
                            {
                                card_index = 0,
                                components = new object[]
                                {
                                    new
                                    {
                                        type = "header",
                                        parameters = new object[]
                                        {
                                            new { type = "image", image = new { id = "" } }
                                        }
                                    }
                                }
                            },
                        }
                    }
                    }
                }
            };

            await MetaSendMessageTemplateAPICall(payload);

        }

    }


}
