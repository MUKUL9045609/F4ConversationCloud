using F4ConversationCloud.Application.Common.Models.CommonModels;
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
    }
}
