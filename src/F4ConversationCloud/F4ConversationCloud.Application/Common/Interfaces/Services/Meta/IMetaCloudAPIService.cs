using F4ConversationCloud.Application.Common.Models.MetaCloudApiModel.Response;
using F4ConversationCloud.Application.Common.Models.MetaCloudApiModel.Templates;
using F4ConversationCloud.Application.Common.Models.MetaModel.Configurations;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using ResponseTemplateMessageCreationResponse = F4ConversationCloud.Application.Common.Models.MetaCloudApiModel.Response.TemplateMessageCreationResponse;


namespace F4ConversationCloud.Application.Common.Interfaces.Services.Meta
{
    public interface IMetaCloudAPIService
    {
        void SetWhatsAppBusinessConfig(WhatsAppBusinessCloudApiConfig cloudApiConfig);
        Task<ResponseTemplateMessageCreationResponse> CreateTemplateMessageAsync(string whatsAppBusinessAccountId, object template, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);
        Task<TemplateResponse> GetAllTemplatesAsync(string whatsAppBusinessAccountId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, string pagingUrl = null, CancellationToken cancellationToken = default);
        Task<TemplateByIdResponse> GetTemplateByIdAsync(string templateId, CancellationToken cancellationToken = default);
        Task<BaseSuccessResponse> DeleteTemplateByIdAsync(string whatsAppBusinessAccountId, string templateId, string templateName, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);


    }
}
