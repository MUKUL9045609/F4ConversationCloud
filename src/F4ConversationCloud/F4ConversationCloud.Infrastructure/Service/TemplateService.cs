using F4ConversationCloud.Application.Common.Interfaces.Services;
using F4ConversationCloud.Application.Common.Interfaces.Services.Meta;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.CommonModels;
using F4ConversationCloud.Application.Common.Models.Templates;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;
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

        public async Task<dynamic> CreateTemplate(MessageTemplateDTO requestBody, string WABAID)
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

                var whatsAppBusinessAccountId = WABAID;

                var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.BaseAddress + WhatsAppBusinessRequestEndpoint.CreateTemplateMessage.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);

                var result = await _logService.CallExternalAPI<dynamic>(formattedWhatsAppEndpoint,
                                                                    methodType,
                                                                    requestBody,
                                                                    headers,
                                                                    "Create Template",
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
                        Message = "Template created successFully."
                    };

                }


            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    Message = "Error occured while creating template.",
                    Status = false,
                    Error = ex.Message,
                    StackTrace = ex.StackTrace
                };
            }
        }
        public async Task<dynamic> EditTemplate(MessageTemplateDTO requestBody, string TemplateId)
        {
            string apiUrl = string.Empty;
            string methodType = "POST";
            var headers = new Dictionary<string, string>();

            try
            {
                string requestJson = JsonConvert.SerializeObject(requestBody);

                string token = "EAAqZAjK5EFEcBPBe6Lfoyi1pMh3cyrQbaBoyHvmLJeyMaZBnb8LsDPTxfdmAgZBcNZBQJpyOqwlQDMBTiMpmzrzZByRyHorE6U76Cffdf7KPzQZAxSEx7YZCMpZBZAN3wU9X1wTpYkrK0w6ZAHdE8SaKNU26js31LfrYB8dsJuQRF2stqwl26qKhJrLTOBUuTcygZDZD";

                headers = new Dictionary<string, string> { { "Authorization", $"Bearer {token}" } };

                var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.BaseAddress + TemplateId;

                var result = await _logService.CallExternalAPI<dynamic>(formattedWhatsAppEndpoint,
                                                                    methodType,
                                                                    requestBody,
                                                                    headers,
                                                                    "Edit Template",
                                                                    null,
                                                                    true);

                if (result.error != null)
                {
                    return new APIResponse
                    {
                        Status = false,
                        result = result,
                        Message = result.error.error_user_msg

                    };
                }
                else
                {
                    return new APIResponse
                    {
                        Status = true,
                        result = result.data,
                        Message = "Template edited successFully."
                    };

                }

            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    Message = "Error occured while editing template.",
                    Status = false,
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
                    Message = "Error occured while deleting template..",
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
                    Message = "Error occured while deleting template.",
                    Success = false,
                    Error = ex.Message,
                    StackTrace = ex.StackTrace
                };
            }
        }

        public async Task<dynamic> SyncTemplateByTemplateID(string TemplateId)
        {
            string apiUrl = string.Empty;
            string methodType = "SyncTemplate";
            var headers = new Dictionary<string, string>();

            try
            {

                string token = "EAAqZAjK5EFEcBPBe6Lfoyi1pMh3cyrQbaBoyHvmLJeyMaZBnb8LsDPTxfdmAgZBcNZBQJpyOqwlQDMBTiMpmzrzZByRyHorE6U76Cffdf7KPzQZAxSEx7YZCMpZBZAN3wU9X1wTpYkrK0w6ZAHdE8SaKNU26js31LfrYB8dsJuQRF2stqwl26qKhJrLTOBUuTcygZDZD";

                headers = new Dictionary<string, string> { { "Authorization", $"Bearer {token}" } };

                var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GraphTemplateSyncApi.ToString().Replace("{{template_id}}", TemplateId);

                var result = await _logService.CallExternalAPI<dynamic>(formattedWhatsAppEndpoint,
                                                                    methodType,
                                                                    null,
                                                                    headers,
                                                                    "Sync Template",
                                                                    null,
                                                                    true);

                return new
                {
                    Success = true,
                    Message = "Template Sync successFully."
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    Message = "Error occured while deleting template.",
                    Success = false,
                    Error = ex.Message,
                    StackTrace = ex.StackTrace
                };
            }
        }
        public MessageTemplateDTO TryDeserializeAndAddComponent(dynamic request)
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

                            var Json = JsonNode.Parse(headJson);

                            if (_typeValue == "image")
                            {

                                var example = Json?["Example"];

                                if (example is not JsonObject exObj)
                                {
                                    Json?.AsObject().Remove("Example");
                                }
                                else
                                {
                                    bool headerFileEmpty = exObj["HeaderFile"] switch
                                    {
                                        JsonArray arr => arr.All(a => a is null),
                                        JsonValue val => string.IsNullOrEmpty(val.GetValue<string>()),
                                        _ => true
                                    };

                                    bool formatEmpty = exObj["Format"] is not JsonValue fv ||
                                                       string.IsNullOrEmpty(fv.GetValue<string>());

                                    if (headerFileEmpty && formatEmpty)
                                    {
                                        Json?.AsObject().Remove("Example");
                                    }
                                }



                                var cleanJson = Json?.ToJsonString();
                                var headersComponent = JsonSerializer.Deserialize<HeadersImageComponent>(cleanJson, options);
                                messageTemplate.components.Add(headersComponent);
                            }
                            else
                            {
                                var Text = Json?["Text"] is JsonArray arr ? arr.FirstOrDefault()?.GetValue<string>()
                                            : Json?["Text"]?.GetValue<string>();



                                var headerTextArray = Json?["Example"]?["Header_Text"]?.AsArray();
                                if (headerTextArray == null || headerTextArray.All(e => e is null))
                                {
                                    Json?.AsObject().Remove("Example");
                                }

                                if (Text != null)
                                {

                                    var cleanJson = Json?.ToJsonString();
                                    var bodyComponent = JsonSerializer.Deserialize<HeadersComponent>(cleanJson, options);
                                    messageTemplate.components.Add(bodyComponent);
                                }
                            }
                        }
                    }
                }

                //BodyComponent
                string BodyJson = JsonSerializer.Serialize(request.TemplateBody);
                using var Bodydoc = JsonDocument.Parse(BodyJson);
                var bodynode = JsonNode.Parse(BodyJson);
                var Bodyroot = Bodydoc.RootElement;

                if (Bodyroot.TryGetProperty("type", out JsonElement BodyElement) || Bodyroot.TryGetProperty("Type", out BodyElement))
                {
                    string typeValue = BodyElement.GetString()?.ToLower();

                    if (typeValue == "body")
                    {
                        var example = bodynode?["Example"];
                        if (example == null ||
                            (example["HeaderFile"]?.GetValue<string>() == null &&
                             example["Format"]?.GetValue<string>() == null))
                        {
                            bodynode?.AsObject().Remove("Example");
                        }
                        var cleanJson = bodynode?.ToJsonString();

                        var bodyComponent = JsonSerializer.Deserialize<BodysComponent>(cleanJson, options);
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
                        var FooterValue = JsonSerializer.Deserialize<FooterComponent>(Footerroot, options);
                        if (!string.IsNullOrEmpty(FooterValue.text))
                        {
                            messageTemplate.components.Add(FooterValue);
                        }
                    }
                }


                //ButtonComponent
                string ButtonsJson = JsonSerializer.Serialize(request.TemplateButton);
                using var Buttonsdoc = JsonDocument.Parse(ButtonsJson);
                var Buttonsnode = JsonNode.Parse(ButtonsJson);
                var Buttonsroot = Buttonsdoc.RootElement;

                if (Buttonsroot.TryGetProperty("type", out JsonElement ButtonsElement) || Buttonsroot.TryGetProperty("Type", out ButtonsElement))
                {
                    string typeValue = ButtonsElement.GetString()?.ToLower();

                    if (typeValue == "buttons")
                    {
                        var example = Buttonsnode?["Buttons"];

                        JsonArray jsonArray = Buttonsnode?[1].AsArray();
                        List<dynamic> Buttoncomponents = new List<dynamic>();
                        for (int i =0; i <jsonArray.Count; i++)
                        {
                            JsonObject buttonObject = jsonArray[i].AsObject();
                            var Type = buttonObject["ButtonActionType"].ToString();


                            if (Type == "QUICK_REPLY")
                            {
                                buttonObject?.AsObject().Remove("Phone_Number");
                                buttonObject?.AsObject().Remove("Url");
                                buttonObject?.AsObject().Remove("Example");

                                var cleanJson = buttonObject?.ToJsonString();

                                var Component = JsonSerializer.Deserialize<QuickReplyButtonComponent>(cleanJson, options);
                                Buttoncomponents.Add(Component);
                            }
                            else if(Type == "PHONE_NUMBER")
                            {
                                buttonObject?.AsObject().Remove("Url");
                                buttonObject?.AsObject().Remove("Example");

                                var cleanJson = buttonObject?.ToJsonString();

                                var Component = JsonSerializer.Deserialize<PhoneNumberButtonComponent>(cleanJson, options);
                                Buttoncomponents.Add(Component);
                            }
                            else if(Type == "URL")
                            {
                                buttonObject?.AsObject().Remove("Phone_Number");

                                var cleanJson = buttonObject?.ToJsonString();

                                var Component = JsonSerializer.Deserialize<UrlButtonComponent>(cleanJson, options);
                                Buttoncomponents.Add(Component);

                            }

                        }

                        var buttons = new
                        {
                            type = "BUTTONS",
                            buttons = Buttoncomponents
                        };

                        if (Buttoncomponents.Count > 0)
                        {
                            messageTemplate.components.Add(buttons);
                        }
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

                //// Step 4: Extract Media ID
                //var secondJson = JObject.Parse(secondResponseString);
                //string mediaId = null;

                //var hValue = secondJson["h"]?.ToString();
                //if (!string.IsNullOrEmpty(hValue))
                //{
                //    var parts = hValue.Split(':');
                //    if (parts.Length >= 3)
                //    {
                //        // WhatsApp media ID is ALWAYS 3rd last element
                //        mediaId = parts[^3];  // ^3 = third item from end
                //    }
                //}


                //if (string.IsNullOrEmpty(mediaId))
                //{
                //    return new
                //    {
                //        Success = false,
                //        Message = "Media ID not found after second upload.",
                //        Response = secondResponseString
                //    };
                //}

                //// Step 5: GET Media URL (this gives the final image path)
                //string mediaUrlApi = $"https://graph.facebook.com/v23.0/{mediaId}?fields=url";

                //var mediaUrlResponse = await client.GetAsync(mediaUrlApi);
                //var mediaUrlResponseString = await mediaUrlResponse.Content.ReadAsStringAsync();

                //if (!mediaUrlResponse.IsSuccessStatusCode)
                //{
                //    return new
                //    {
                //        Success = false,
                //        Message = "Failed to fetch media URL.",
                //        StatusCode = mediaUrlResponse.StatusCode,
                //        Response = mediaUrlResponseString
                //    };
                //}

                //var mediaJson = JObject.Parse(mediaUrlResponseString);
                //var finalImageUrl = mediaJson["url"]?.ToString();

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


        public async Task<dynamic> Whatsappbusinessprofile(string profilepicturehandle, string PhoneNumberId)
        {
            string apiUrl = string.Empty;
            string methodType = "POST";
            var headers = new Dictionary<string, string>();

            try
            {
                var requestBody = new
                {
                    messaging_product = "whatsapp",
                    profile_picture_handle = profilepicturehandle
                };

                string requestJson = JsonConvert.SerializeObject(requestBody);


                string token = "EAAqZAjK5EFEcBPBe6Lfoyi1pMh3cyrQbaBoyHvmLJeyMaZBnb8LsDPTxfdmAgZBcNZBQJpyOqwlQDMBTiMpmzrzZByRyHorE6U76Cffdf7KPzQZAxSEx7YZCMpZBZAN3wU9X1wTpYkrK0w6ZAHdE8SaKNU26js31LfrYB8dsJuQRF2stqwl26qKhJrLTOBUuTcygZDZD";

                headers = new Dictionary<string, string> { { "Authorization", $"Bearer {token}" } };


                var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.BaseAddress + WhatsAppBusinessRequestEndpoint.Whatsappbusinessprofile.Replace("{{Phone-Number-ID}}", PhoneNumberId);

                var result = await _logService.CallExternalAPI<dynamic>(formattedWhatsAppEndpoint,
                                                                    methodType,
                                                                    requestBody,
                                                                    headers,
                                                                    "Updatewhatsapp business profile",
                                                                    null,
                                                                    true);

                return new
                {
                    Success = true,
                    Message = "Whatsapp Profile created successFully."

                };
            }
            catch (Exception ex)
            {
                return new
                {
                    Message = "Error occured while creating template.",
                    Success = false,
                    Error = ex.Message,
                    StackTrace = ex.StackTrace
                };
            }
        }


        public async Task<dynamic> GetWhatsappbusinessprofile(string PhoneNumberId)
        {
            string apiUrl = string.Empty;
            string methodType = "POST";
            var headers = new Dictionary<string, string>();
            var requestBody = string.Empty;

            try
            {
                string requestJson = JsonConvert.SerializeObject(requestBody);

                string token = "EAAqZAjK5EFEcBPBe6Lfoyi1pMh3cyrQbaBoyHvmLJeyMaZBnb8LsDPTxfdmAgZBcNZBQJpyOqwlQDMBTiMpmzrzZByRyHorE6U76Cffdf7KPzQZAxSEx7YZCMpZBZAN3wU9X1wTpYkrK0w6ZAHdE8SaKNU26js31LfrYB8dsJuQRF2stqwl26qKhJrLTOBUuTcygZDZD";

                headers = new Dictionary<string, string> { { "Authorization", $"Bearer {token}" } };

                var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.BaseAddress + WhatsAppBusinessRequestEndpoint.Whatsappbusinessprofile.Replace("{{Phone-Number-ID}}", PhoneNumberId) + "whatsapp_business_profile?fields=profile_picture_url";

                var result = await _logService.CallExternalAPI<dynamic>(formattedWhatsAppEndpoint,
                                                                    methodType,
                                                                    requestBody,
                                                                    headers,
                                                                    "Get Meta Whatsapp business profile",
                                                                    null,
                                                                    true);
                return new
                {
                    Success = true,
                    Message = "Whatsapp Profile created successFully."

                };
            }
            catch (Exception ex)
            {
                return new
                {
                    Message = "Error occured while creating template.",
                    Success = false,
                    Error = ex.Message,
                    StackTrace = ex.StackTrace
                };
            }
        }

        public async Task<dynamic> GetAllTemplatesAsync(string wabaId)
        {
            string apiUrl = string.Empty;
            string methodType = "Get";
            var headers = new Dictionary<string, string>();
            var requestBody = string.Empty;

            try
            {
                var allTemplates = new List<WhatsappTemplateDetail>();

                string token = "EAAqZAjK5EFEcBPBe6Lfoyi1pMh3cyrQbaBoyHvmLJeyMaZBnb8LsDPTxfdmAgZBcNZBQJpyOqwlQDMBTiMpmzrzZByRyHorE6U76Cffdf7KPzQZAxSEx7YZCMpZBZAN3wU9X1wTpYkrK0w6ZAHdE8SaKNU26js31LfrYB8dsJuQRF2stqwl26qKhJrLTOBUuTcygZDZD";

                headers = new Dictionary<string, string> { { "Authorization", $"Bearer {token}" } };

                var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GraphTemplateSyncApi.ToString().Replace("{{waba_id}}", wabaId).Replace("{{access_token}}", token);
                
                string requestJson = formattedWhatsAppEndpoint;

                while (!string.IsNullOrEmpty(formattedWhatsAppEndpoint))
                {

                    var result = await _logService.CallExternalAPI<dynamic>(formattedWhatsAppEndpoint,
                                                                        methodType,
                                                                        requestBody,
                                                                        headers,
                                                                        "Get Meta Whatsapp business profile",
                                                                        null,
                    true);



                    var data = result["data"];

                    foreach (var item in data)
                    {
                        allTemplates.Add(new WhatsappTemplateDetail
                        {
                            TemplateName = item["name"]?.ToString(),
                            LanguageCode = item["language"]?.ToString(),
                            Category = item["category"]?.ToString(),
                            Status = item["status"]?.ToString(),
                            TemplateId = item["id"]?.ToString()
                        });
                    }

                    var paging = result["paging"];
                    if (paging != null && paging["next"] != null)
                    {
                        formattedWhatsAppEndpoint = paging["next"]?.ToString();
                    }
                    else
                    {
                        formattedWhatsAppEndpoint = null;
                    }
                }

                return allTemplates;
            }
            catch (Exception ex)
            {
                return new
                {
                    Message = "Error occured while creating template.",
                    Success = false,
                    Error = ex.Message,
                    StackTrace = ex.StackTrace
                };
            }
        }



    }



}
