using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models.MetaCloudApiModel.Templates;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Infrastructure.Service.SuperAdmin
{
    public class TemplateManagementService: ITemplateManagementService
    {
        private readonly HttpClient _httpClient;
        private IConfiguration _configuration { get; }
        public TemplateManagementService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task<TemplateMessageCreationResponse> CreateTemplate(WhatsAppTemplateRequest request)
        {
            try
            {
                var WABAId = _configuration["WhatsAppBusinessCloudApiConfiguration:WhatsAppBusinessAccountId"];
                var accessToken = _configuration["WhatsAppBusinessCloudApiConfiguration:AccessToken"];

                var convertedComponents = request.Components.Select(c => new WhatsAppBusinessHSMWhatsAppHSMComponentGet
                {
                    Type = c.Type,
                    Text = c.Text,
                    Format = c.Type == "HEADER" ? c.Format : null,
                    Example = new Example
                    {
                        BodyText = c.Example?.BodyText ?? new List<List<string>>()
                    }
                }).ToList();

                var template = new TemplateData
                {
                    Name = request.Name,
                    Language = request.Language,
                    Category = request.Category,
                    Components = convertedComponents
                };

                return null;
            }
            catch (Exception)
            {

                throw;
            }




        }
    }
}
