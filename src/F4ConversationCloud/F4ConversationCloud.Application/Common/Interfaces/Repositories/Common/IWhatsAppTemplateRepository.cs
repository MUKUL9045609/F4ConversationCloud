using F4ConversationCloud.Application.Common.Models.ClientModel;
using F4ConversationCloud.Application.Common.Models.CommonModels;
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
        Task<IEnumerable<TemplateModel>> GetFilteredAsync(TemplateListFilter filter);
        Task<int> GetCountAsync(TemplateListFilter filter);
    }
}
