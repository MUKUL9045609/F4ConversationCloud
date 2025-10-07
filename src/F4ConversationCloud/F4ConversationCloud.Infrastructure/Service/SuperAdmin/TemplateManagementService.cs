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
        private IClientManagementService _clientManagement;
        public TemplateManagementService(HttpClient httpClient, IConfiguration configuration, IMetaCloudAPIService metaCloudAPI, WhatsAppBusinessCloudApiConfig whatsAppBusinessCloud, IClientManagementService clientManagement)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _metaCloudAPI = metaCloudAPI;
            _whatsAppConfig = whatsAppBusinessCloud;
            _clientManagement = clientManagement;
        }
        public async Task<TemplateMessageCreationResponse> CreateTemplate(WhatsAppTemplateRequest request)
        {
            try
            {

                //var wabaId = _whatsAppConfig.WhatsAppBusinessAccountId;
                var clientDetails = await _clientManagement.GetClientDetailsById(request.ClientInfoId); 
                
                if (clientDetails != null )
                {
                  
                    var metaApiResponce = await _metaCloudAPI.CreateTemplateMessageAsync(clientDetails.WABAId, request);
                }

                

                return null;
            }
            catch (Exception)
            {

                return null;
            }




        }
        public async Task<TemplateListViewItem> TemplateListAsync(TemplatesListFilter filter)
        {
            try
            {
                var clientDetails = await _clientManagement.GetClientDetailsById(filter.ClientInfoId);
            
                if (clientDetails != null)
                {
                    var GetTemplateListesponse = await _metaCloudAPI.GetAllTemplatesAsync(clientDetails.WABAId);

                    var Templatelist = new TemplateResponse
                    {
                        Data = GetTemplateListesponse.Data
                    };

                    return new TemplateListViewItem
                    {
                        Data = Templatelist.Data,

                    };

                }
                {
                    return new  TemplateListViewItem();
                }
            }
            catch (Exception)
            {

                return new TemplateListViewItem();
            }
            
                
        }
    }
}
