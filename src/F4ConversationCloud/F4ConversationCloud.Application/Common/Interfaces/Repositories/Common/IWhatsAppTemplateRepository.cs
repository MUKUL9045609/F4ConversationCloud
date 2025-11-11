using F4ConversationCloud.Application.Common.Models.CommonModels;
using F4ConversationCloud.Application.Common.Models.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Interfaces.Repositories.Common
{
    public interface IWhatsAppTemplateRepository
    {
        Task<(IEnumerable<WhatsappTemplateListItem> Templates, int TotalCount)> GetTemplatesListAsync(WhatsappTemplateListFilter filter);
        Task<WhatsappTemplateDetail> GetTemplateByIdAsync(string Template_id);
        Task<int> InsertTemplatesListAsync(MessageTemplateDTO request, string TemplateId, string ClientInfoId, string CreatedBy, string WABAID);

        Task<int> UpdateTemplatesAsync(MessageTemplateDTO request, string TemplateId);
    }
}
