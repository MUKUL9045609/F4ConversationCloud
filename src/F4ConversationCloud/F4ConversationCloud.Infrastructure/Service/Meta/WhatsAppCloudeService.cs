using F4ConversationCloud.Application.Common.Interfaces.Services.Meta;
using F4ConversationCloud.Application.Common.Meta.BussinessProfile;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace F4ConversationCloud.Infrastructure.Service.MetaServices
{
    public class WhatsAppCloudeService : IWhatsAppCloudeService
    {
        private readonly HttpClient _httpClient;
        private IConfiguration _configuration { get; }
        public WhatsAppCloudeService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<WhatsAppPhoneNumberInfoViewModel> GetWhatsAppPhoneNumberDetailsAsync(string phoneNumberId)
        {
            try
            {
                var baseUrl = _configuration["WhatsAppAPISettings:FacebookGraphMessageEndpoint"];
                var accessToken = _configuration["WhatsAppAPISettings:AccessToken"];

                var requestUrl = $"{baseUrl}/{phoneNumberId}?fields=id,display_phone_number,verified_name,status&access_token={accessToken}";

                using var client = new HttpClient();
                var response = await client.GetAsync(requestUrl);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    response.EnsureSuccessStatusCode();
                }

                var content = await response.Content.ReadAsStringAsync();

                //var options = new JsonSerializerOptions
                //{
                //    PropertyNameCaseInsensitive = true
                //};

                var info = JsonSerializer.Deserialize<WhatsAppPhoneNumberInfoViewModel>(content);

                return info ?? new WhatsAppPhoneNumberInfoViewModel();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
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




    }
}
