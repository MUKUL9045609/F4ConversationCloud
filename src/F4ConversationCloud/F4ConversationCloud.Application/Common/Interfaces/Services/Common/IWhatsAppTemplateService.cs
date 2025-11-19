using F4ConversationCloud.Application.Common.Models.CommonModels;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Interfaces.Services.Common
{
    public interface IWhatsAppTemplateService
    {
        Task<WhatsAppTemplateResponse> GetTemplatesListAsync(WhatsappTemplateListFilter filter);
        Task<WhatsappTemplateDetail> GetTemplateByIdAsync(int Template_id);
        Task<Tuple<IEnumerable<TemplateModel>, int>> GetFilteredTemplatesByWABAId(TemplateListFilter filter);
        Task<DeleteTemplateResponse> DeactivateTemplateAsync(int templateId);
        Task<DeleteTemplateResponse> ActivateTemplateAsync(int templateId);
        Task<IEnumerable<TemplateModel.Button>> GetTemplateButtonsAsync(int id);
    }
}
