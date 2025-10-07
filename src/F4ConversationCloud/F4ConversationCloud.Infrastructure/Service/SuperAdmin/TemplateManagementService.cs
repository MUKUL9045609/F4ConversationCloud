using F4ConversationCloud.Application.Common.Interfaces.Services.Meta;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models.MetaCloudApiModel.Templates;
using F4ConversationCloud.Application.Common.Models.MetaModel.Configurations;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using Microsoft.Extensions.Configuration;


namespace F4ConversationCloud.Infrastructure.Service.SuperAdmin
{
    public class TemplateManagementService : ITemplateManagementService
    {
        private readonly HttpClient _httpClient;
        private WhatsAppBusinessCloudApiConfig _whatsAppConfig;
        private IConfiguration _configuration { get; }
        private IMetaCloudAPIService _metaCloudAPI;
        public TemplateManagementService(HttpClient httpClient, IConfiguration configuration, IMetaCloudAPIService metaCloudAPI, WhatsAppBusinessCloudApiConfig whatsAppBusinessCloud)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _metaCloudAPI = metaCloudAPI;
            _whatsAppConfig = whatsAppBusinessCloud;
        }
        public async Task<TemplateMessageCreationResponse> CreateTemplate(WhatsAppTemplateRequest request)
        {
            try
            {

                var wabaId = _whatsAppConfig.WhatsAppBusinessAccountId;
                var metaApiResponce = await _metaCloudAPI.CreateTemplateMessageAsync(wabaId, request);

                return null;
            }
            catch (Exception)
            {

                return null;
            }




        }
        public async Task<TemplateListViewItem> TemplateListAsync(TemplatesListFilter filter)
        {
            var wabaId = _whatsAppConfig.WhatsAppBusinessAccountId; 

            var GetTemplateListesponse = await _metaCloudAPI.GetAllTemplatesAsync(wabaId);

                var Templatelist = new TemplateResponse
                {
                    Data = GetTemplateListesponse.Data
                };

            return new TemplateListViewItem
            {
                Data = Templatelist.Data,
                
            };
        }
    }
}
