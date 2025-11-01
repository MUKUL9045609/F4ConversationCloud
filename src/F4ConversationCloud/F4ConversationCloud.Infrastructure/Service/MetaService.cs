using F4ConversationCloud.Application.Common.Interfaces.Services;
using F4ConversationCloud.Application.Common.Interfaces.Services.Meta;
using F4ConversationCloud.Application.Common.Meta.BussinessProfile;
using F4ConversationCloud.Application.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace F4ConversationCloud.Infrastructure.Service
{
    public class MetaService : IMetaService
    {
        private IConfiguration _configuration;
        private ITemplateService _templateService;

        public MetaService(IConfiguration configuration , ITemplateService templateService) { 
            _configuration = configuration;
            _templateService = templateService;
        }

        public async Task<PhoneRegistrationOnMetaResponse> RegisterPhone(PhoneRegistrationOnMeta request)
        {
            try
            {
                var baseUrl = _configuration["WhatsAppAPISettings:FacebookGraphMessageEndpoint"];
                var accessToken = _configuration["WhatsAppAPISettings:Token"];

                string requestUrl = $"{baseUrl}/{Uri.EscapeDataString(request.PhoneNumberId)}/register?access_token={Uri.EscapeDataString(accessToken)}";

                var questPayload = new
                {
                    messaging_product = request.messaging_product,
                    pin = request.Pin
                };

                var jsonContent = System.Text.Json.JsonSerializer.Serialize(questPayload);
                using var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                using var client = new HttpClient();

                var response = await client.PostAsync(requestUrl, content);
                var contents = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new PhoneRegistrationOnMetaResponse { status = false };
                }


                var jsonDoc = System.Text.Json.JsonDocument.Parse(contents);
                bool success = false;

                if (jsonDoc.RootElement.TryGetProperty("success", out var successProp))
                {
                    success = successProp.GetBoolean();
                }

                return new PhoneRegistrationOnMetaResponse { status = success };
            }
            catch (Exception)
            {
                return new PhoneRegistrationOnMetaResponse { status = false };
            }
        }

        public async Task<PhoneRegistrationOnMetaResponse> DeregisterPhone(PhoneRegistrationOnMeta request)
        {
            try
            {
                var baseUrl = _configuration["WhatsAppAPISettings:FacebookGraphMessageEndpoint"];
                var accessToken = _configuration["WhatsAppAPISettings:AccessToken"];

                string requestUrl = $"{baseUrl}/{Uri.EscapeDataString(request.PhoneNumberId)}/deregister?access_token={Uri.EscapeDataString(accessToken)}";

                var questPayload = new
                {
                    messaging_product = request.messaging_product,
                    pin = request.Pin
                };

                var jsonContent = System.Text.Json.JsonSerializer.Serialize(questPayload);
                using var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                using var client = new HttpClient();

                var response = await client.PostAsync(requestUrl, content);
                response.EnsureSuccessStatusCode();

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();

                }

                var contents = await response.Content.ReadAsStringAsync();



                PhoneRegistrationOnMetaResponse PhoneRegistrationOnMetaResponse = System.Text.Json.JsonSerializer.Deserialize<PhoneRegistrationOnMetaResponse>(contents, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return PhoneRegistrationOnMetaResponse ?? new PhoneRegistrationOnMetaResponse();
            }
            catch (Exception)
            {
                return new PhoneRegistrationOnMetaResponse();
            }
        }


        public async Task<IActionResult> Updatewhatsappbusinessprofile(IFormFile file)
        {
            try
            {

                if (file == null || file.Length == 0)
                    return null;

                if (!file.ContentType.StartsWith("image/"))
                    return null;

                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                byte[] fileBytes = memoryStream.ToArray();

                string base64 = Convert.ToBase64String(fileBytes);
                string dataUri = $"data:{file.ContentType};base64,{base64}";

                string headerFileJsonString = await _templateService.UploadMetaImage(dataUri);
                using JsonDocument doc = JsonDocument.Parse(headerFileJsonString);
                JsonElement root = doc.RootElement;
                string hValue = "";
                string PhoneNumberId = "";


                if (root.TryGetProperty("h", out JsonElement hProperty))
                {
                    hValue = hProperty.GetString();
                }

                var phone = await _templateService.Whatsappbusinessprofile(hValue, PhoneNumberId);

                //return Ok(new
                //{
                //    file.FileName,
                //    file.ContentType,
                //    hValue = hValue
                //});
                return null;
            }
            catch (Exception ex)
            {
                return null;
                //return Ok();
            }
        }

    }
}
