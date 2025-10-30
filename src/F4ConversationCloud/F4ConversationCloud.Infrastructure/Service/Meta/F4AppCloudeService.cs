using F4ConversationCloud.Application.Common.Interfaces.Services.Meta;
using F4ConversationCloud.Application.Common.Meta.BussinessProfile;
using F4ConversationCloud.Application.Common.Models.MetaCloudApiModel.Templates;
using F4ConversationCloud.Application.Common.Models.MetaModel.BussinessProfile;
using F4ConversationCloud.Domain.Entities;
using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Twilio.Types;


namespace F4ConversationCloud.Infrastructure.Service.MetaServices
{
    public class F4AppCloudeService : IF4AppCloudeService
    {
        private readonly HttpClient _httpClient;
        private IConfiguration _configuration { get; }
        public F4AppCloudeService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<WhatsAppPhoneNumberInfoViewModel> GetWhatsAppPhoneNumberDetailsAsync(string phoneNumberId)
        {
            try
            {
                var baseUrl = _configuration["WhatsAppBusinessCloudApiConfiguration:FacebookGraphMessageEndpoint"];
                var accessToken = _configuration["WhatsAppBusinessCloudApiConfiguration:AccessToken"];

                string requestUrl = $"{baseUrl}/{Uri.EscapeDataString(phoneNumberId)}" +
                                    "?fields=id,display_phone_number,verified_name,whatsapp_business_profile{email,websites,vertical},status" +
                                    $"&access_token={Uri.EscapeDataString(accessToken)}";

                using var client = new HttpClient();
                var response = await client.GetAsync(requestUrl);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    
                }

                var content = await response.Content.ReadAsStringAsync();

                var info = JsonSerializer.Deserialize<WhatsAppPhoneNumberInfoViewModel>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                string vertical = info?.WhatsAppBusinessProfile?.Data?.FirstOrDefault()?.Vertical;
                string email = info?.WhatsAppBusinessProfile?.Data?.FirstOrDefault() ?.Email;
                List<string> websites = info?.WhatsAppBusinessProfile?.Data?.FirstOrDefault()?.Websites;
               


                return info ?? new WhatsAppPhoneNumberInfoViewModel();
            }
            catch (Exception)
            {
                return new WhatsAppPhoneNumberInfoViewModel();
            }
        }


        public List<string> ExtractTemplateParameters(string templateBodyText)
        {
            var matches = Regex.Matches(templateBodyText, @"\{\{(.*?)\}\}");
            return matches.Cast<Match>()
                          .Select(m => m.Groups[1].Value.Trim())
                          .Distinct()
                          .ToList();
        }

        public async Task<ActivateClientAccountResponse> RegisterClientAccountAsync(ActivateClientAccountRequest request)
        {
            try
            {
                var baseUrl = _configuration["WhatsAppAPISettings:FacebookGraphMessageEndpoint"];
                var accessToken = _configuration["WhatsAppAPISettings:Token"];

                string requestUrl = $"{baseUrl}/{Uri.EscapeDataString(request.PhoneNumberId)}/register?access_token={Uri.EscapeDataString(accessToken)}";

                var questPayload = new
                {
                    messaging_product = "whatsapp",
                    pin = "313466"
                };

                var jsonContent = System.Text.Json.JsonSerializer.Serialize(questPayload);
                using var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                using var client = new HttpClient();

                var response = await client.PostAsync(requestUrl, content);
                var contents = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new ActivateClientAccountResponse { status = false };
                }

                
                var jsonDoc = System.Text.Json.JsonDocument.Parse(contents);
                bool success = false;

                if (jsonDoc.RootElement.TryGetProperty("success", out var successProp))
                {
                    success = successProp.GetBoolean();
                }

                return new ActivateClientAccountResponse { status = success };
            }
            catch (Exception)
            {
                return new ActivateClientAccountResponse { status = false };
            }
        }

    }
}
