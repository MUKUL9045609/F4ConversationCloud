using F4ConversationCloud.Application.Common.Models.MetaCloudApiModel.Templates;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin
{
    public interface ITemplateManagementService
    {
        Task<TemplateMessageCreationResponse> CreateTemplate(WhatsAppTemplateRequest request);
        Task<TemplateByIdResponse> GetTemplateByIdAsync(string Template_id);
        Task<TemplateListViewItem> TemplateListAsync(TemplatesListFilter filter);
        Task<DeleteTemplateResponse> DeleteTemplateById(string Template_id, int ClientInfoId, string TemplateName);
    }
}
