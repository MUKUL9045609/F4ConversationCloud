﻿using F4ConversationCloud.Application.Common.Interfaces.Services;
using F4ConversationCloud.Application.Common.Interfaces.Services.Meta;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models.Templates;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace F4ConversationCloud.Infrastructure.Service
{
    public class TemplateService : ITemplateService
    {
        private readonly IAPILogService _logService;
        private IConfiguration _configuration { get; }

        public TemplateService(IClientManagementService clientManagement, IAPILogService logService)
        {
            _logService = logService;
        }

        public async Task<dynamic> CreateTemplate(MessageTemplateDTO requestBody)
        {
            string apiUrl = string.Empty;
            string methodType = "POST";
            var headers = new Dictionary<string, string>();

            try
            {
                string requestJson = JsonConvert.SerializeObject(requestBody);

                //string token = _configuration["WhatsAppAPISettings:Token"];
                string token = "EAAqZAjK5EFEcBPBe6Lfoyi1pMh3cyrQbaBoyHvmLJeyMaZBnb8LsDPTxfdmAgZBcNZBQJpyOqwlQDMBTiMpmzrzZByRyHorE6U76Cffdf7KPzQZAxSEx7YZCMpZBZAN3wU9X1wTpYkrK0w6ZAHdE8SaKNU26js31LfrYB8dsJuQRF2stqwl26qKhJrLTOBUuTcygZDZD";
                string WABAID = "528970240291210";

                headers = new Dictionary<string, string> { { "Authorization", $"Bearer {token}" } };

                var whatsAppBusinessAccountId = WABAID;

                var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.BaseAddress + WhatsAppBusinessRequestEndpoint.CreateTemplateMessage.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);

                var result = await _logService.CallExternalAPI<dynamic>(formattedWhatsAppEndpoint,
                                                                    methodType,
                                                                    requestBody,
                                                                    headers,
                                                                    "Create Template",
                                                                    null,
                                                                    true);

                return new
                {
                    Success = true,
                    Message = "Template created successFully."

                };
            }
            catch (Exception ex)
            {
                return new
                {
                    Message = "Template not created successFully.",
                    Success = false,
                    Error = ex.Message,
                    StackTrace = ex.StackTrace
                };
            }
        }
        public async Task<dynamic> EditTemplate(MessageTemplateDTO requestBody, string TemplateID)
        {
            string apiUrl = string.Empty;
            string methodType = "POST";
            var headers = new Dictionary<string, string>();

            try
            {
                string requestJson = JsonConvert.SerializeObject(requestBody);

                string token = "EAAqZAjK5EFEcBPBe6Lfoyi1pMh3cyrQbaBoyHvmLJeyMaZBnb8LsDPTxfdmAgZBcNZBQJpyOqwlQDMBTiMpmzrzZByRyHorE6U76Cffdf7KPzQZAxSEx7YZCMpZBZAN3wU9X1wTpYkrK0w6ZAHdE8SaKNU26js31LfrYB8dsJuQRF2stqwl26qKhJrLTOBUuTcygZDZD";
                string WABAID = "528970240291210";

                headers = new Dictionary<string, string> { { "Authorization", $"Bearer {token}" } };

                var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.BaseAddress + TemplateID;

                var result = await _logService.CallExternalAPI<dynamic>(formattedWhatsAppEndpoint,
                                                                    methodType,
                                                                    requestBody,
                                                                    headers,
                                                                    "Edit Template",
                                                                    null,
                                                                    true);

                return new
                {
                    Success = true,
                    Message = "Template edited successFully."
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    Message = "Template not edited.",
                    Success = false,
                    Error = ex.Message,
                    StackTrace = ex.StackTrace
                };
            }
        }
        public async Task<dynamic> DeleteTemplate(int TemplateId, string TemplateName)
        {
            string apiUrl = string.Empty;
            string methodType = "DELETE";
            var headers = new Dictionary<string, string>();

            try
            {

                string token = _configuration["WhatsAppAPISettings:FacebookGraphMessageTemplatesEndpoint"];
                string WABAID = _configuration["WhatsAppAPISettings:FacebookGraphMessageTemplatesEndpoint"];

                headers = new Dictionary<string, string> { { "Authorization", $"Bearer {token}" } };

                var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GraphDeleteApiVersionBaseAddress.ToString().Replace("{{hsm_id}}", TemplateId.ToString()).Replace("{{name}}", TemplateName);

                var result = await _logService.CallExternalAPI<dynamic>(formattedWhatsAppEndpoint,
                                                                    methodType,
                                                                    null,
                                                                    headers,
                                                                    "Delete Template",
                                                                    null,
                                                                    true);

                return new
                {
                    Success = true,
                    Message = "Template deletd successFully."
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    Message = "Template not deletd.",
                    Success = false,
                    Error = ex.Message,
                    StackTrace = ex.StackTrace
                };
            }
        }
        public async Task<dynamic> DeleteTemplateByName(string TemplateName)
        {
            string apiUrl = string.Empty;
            string methodType = "DELETE";
            var headers = new Dictionary<string, string>();

            try
            {

                string token = _configuration["WhatsAppAPISettings:FacebookGraphMessageTemplatesEndpoint"];
                string WABAID = _configuration["WhatsAppAPISettings:FacebookGraphMessageTemplatesEndpoint"];

                headers = new Dictionary<string, string> { { "Authorization", $"Bearer {token}" } };

                var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GraphDeleteApiVersionBaseAddress.ToString().Replace("{{name}}", TemplateName);

                var result = await _logService.CallExternalAPI<dynamic>(formattedWhatsAppEndpoint,
                                                                    methodType,
                                                                    null,
                                                                    headers,
                                                                    "Delete Template",
                                                                    null,
                                                                    true);

                return new
                {
                    Success = true,
                    Message = "Template deletd successFully."
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    Message = "Template not deletd.",
                    Success = false,
                    Error = ex.Message,
                    StackTrace = ex.StackTrace
                };
            }
        }
        public MessageTemplateDTO TryDeserializeAndAddComponent(TemplateRequest request)
        {
            try
            {
                var messageTemplate = new MessageTemplateDTO
                {
                    name = request.Name,
                    language = request.Language,
                    category = request.Category,
                    components = new List<dynamic>()
                };

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };


                //HeaderComponent

                string headJson = JsonSerializer.Serialize(request.TemplateHeader);
                using var headdoc = JsonDocument.Parse(headJson);
                var headroot = headdoc.RootElement;

                if (headroot.TryGetProperty("type", out JsonElement typeheadElement) || headroot.TryGetProperty("Type", out typeheadElement))
                {
                    string typeValue = typeheadElement.GetString()?.ToLower();

                    if (typeValue == "header")
                    {

                        if (headroot.TryGetProperty("format", out JsonElement typehead) || headroot.TryGetProperty("Format", out typehead))
                        {
                            string _typeValue = typehead.GetString()?.ToLower();
                            if (_typeValue == "image")
                            {
                                var headersComponent = JsonSerializer.Deserialize<HeadersImageComponent>(headJson, options);
                                messageTemplate.components.Add(headersComponent);
                            }
                            else
                            {
                                var bodyComponent = JsonSerializer.Deserialize<HeadersComponent>(headJson, options);
                                messageTemplate.components.Add(bodyComponent);
                            }

                        }
                    }
                }

                //BodyComponent
                string BodyJson = JsonSerializer.Serialize(request.TemplateBody);
                using var Bodydoc = JsonDocument.Parse(BodyJson);
                var Bodyroot = Bodydoc.RootElement;

                if (Bodyroot.TryGetProperty("type", out JsonElement BodyElement) || Bodyroot.TryGetProperty("Type", out BodyElement))
                {
                    string typeValue = BodyElement.GetString()?.ToLower();

                    if (typeValue == "body")
                    {
                        var bodyComponent = JsonSerializer.Deserialize<BodysComponent>(BodyJson, options);
                        messageTemplate.components.Add(bodyComponent);

                    }
                }


                //FooterComponent
                string FooterJson = JsonSerializer.Serialize(request.TemplateFooter);
                using var Footerdoc = JsonDocument.Parse(FooterJson);
                var Footerroot = Footerdoc.RootElement;

                if (Footerroot.TryGetProperty("type", out JsonElement FooterElement) || Footerroot.TryGetProperty("Type", out FooterElement))
                {
                    string typeValue = FooterElement.GetString()?.ToLower();

                    if (typeValue == "footer")
                    {
                        messageTemplate.components.Add(JsonSerializer.Deserialize<FootersComponent>(Footerroot, options));

                    }
                }


                //ButtonComponent
                string ButtonsJson = JsonSerializer.Serialize(request.TemplateButton);
                using var Buttonsdoc = JsonDocument.Parse(ButtonsJson);
                var Buttonsroot = Buttonsdoc.RootElement;

                if (Buttonsroot.TryGetProperty("type", out JsonElement ButtonsElement) || Buttonsroot.TryGetProperty("Type", out ButtonsElement))
                {
                    string typeValue = ButtonsElement.GetString()?.ToLower();

                    if (typeValue == "buttons")
                    {
                        var Component = JsonSerializer.Deserialize<ButtonsComponent>(Buttonsroot, options);
                        messageTemplate.components.Add(Component);

                    }
                }

                return messageTemplate;
                //await CreateTemplate(messageTemplate);
            }
            catch (Exception ex)
            {
                return new MessageTemplateDTO();
            }
        }


        public MessageTemplateDTO TryDeserializeComponent(TemplateRequest request)
        {
            var messageTemplate = new MessageTemplateDTO
            {
                name = request.Name,
                language = request.Language,
                category = request.Category,
                components = new List<dynamic>()
            };

            if (request.TemplateBody == null)
            {
                return messageTemplate;
            }

            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                string templateBodyJson = JsonSerializer.Serialize(request.TemplateBody);
                using var templateBodyDoc = JsonDocument.Parse(templateBodyJson);
                var templateBodyRoot = templateBodyDoc.RootElement;

                if (templateBodyRoot.TryGetProperty("type", out JsonElement typeElement))
                {
                    string typeValue = typeElement.GetString()?.ToLower();

                    switch (typeValue)
                    {
                        case "header":
                            var headerComponent = templateBodyRoot.Deserialize<HeadersComponent>(options);
                            messageTemplate.components.Add(headerComponent);
                            break;
                        case "body":
                            var bodyComponent = templateBodyRoot.Deserialize<BodyComponent>(options);
                            messageTemplate.components.Add(bodyComponent);
                            break;
                        case "footer":
                            var footerComponent = templateBodyRoot.Deserialize<FooterComponent>(options);
                            messageTemplate.components.Add(footerComponent);
                            break;
                        case "buttons":
                            var buttonsComponent = templateBodyRoot.Deserialize<ButtonsComponent>(options);
                            messageTemplate.components.Add(buttonsComponent);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"An error occurred during deserialization: {ex.Message}");
                return new MessageTemplateDTO();
            }

            return messageTemplate;
        }

        public async Task<dynamic> UploadMetaImage(string base64Image)
        {
            try
            {
                string accessToken = "EAAqZAjK5EFEcBPBe6Lfoyi1pMh3cyrQbaBoyHvmLJeyMaZBnb8LsDPTxfdmAgZBcNZBQJpyOqwlQDMBTiMpmzrzZByRyHorE6U76Cffdf7KPzQZAxSEx7YZCMpZBZAN3wU9X1wTpYkrK0w6ZAHdE8SaKNU26js31LfrYB8dsJuQRF2stqwl26qKhJrLTOBUuTcygZDZD";
                // Step 1: Decode base64
                var base64Data = base64Image.Substring(base64Image.IndexOf(",") + 1);
                var imageBytes = Convert.FromBase64String(base64Data);
                var fileLength = imageBytes.Length;
                var fileName = "upload.jpg";
                var fileType = "image/jpeg";

                // Step 2: POST to /app/uploads
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                using var content = new MultipartFormDataContent();
                var fileContent = new ByteArrayContent(imageBytes);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(fileType);
                content.Add(fileContent, "file", fileName);
                content.Add(new StringContent(fileLength.ToString()), "file_length");
                content.Add(new StringContent(fileType), "file_type");
                content.Add(new StringContent(fileName), "file_name");

                var uploadResponse = await client.PostAsync("https://graph.facebook.com/v23.0/app/uploads", content);
                var uploadResponseString = await uploadResponse.Content.ReadAsStringAsync();

                if (!uploadResponse.IsSuccessStatusCode)
                {
                    return new
                    {
                        Success = false,
                        Message = "Failed at /app/uploads",
                        StatusCode = uploadResponse.StatusCode,
                        Response = uploadResponseString
                    };
                }

                var uploadJson = JObject.Parse(uploadResponseString);
                var uploadId = uploadJson["id"]?.ToString();
                if (string.IsNullOrEmpty(uploadId))
                {
                    return new
                    {
                        Success = false,
                        Message = "Upload ID not found in response."
                    };
                }

                // Step 3: POST to /upload:<id>
                string secondUrl = $"https://graph.facebook.com/v23.0/{uploadId}";

                var secondResponse = await client.PostAsync(secondUrl, new ByteArrayContent(imageBytes));

                var secondResponseString = await secondResponse.Content.ReadAsStringAsync();

                if (!secondResponse.IsSuccessStatusCode)
                {
                    return new
                    {
                        Success = false,
                        Message = "Failed at second upload step",
                        StatusCode = secondResponse.StatusCode,
                        Response = secondResponseString
                    };
                }

                return secondResponseString;

            }
            catch (Exception ex)
            {
                return new
                {
                    Success = false,
                    Message = "Exception occurred during image upload.",
                    Error = ex.Message,
                    StackTrace = ex.StackTrace
                };
            }
        }
    }


}
