using F4ConversationCloud.Application.Common.Models.MetaCloudApiModel.Templates;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Domain.Entities.SuperAdmin;
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

        Task<TemplateListViewItem> TemplateListAsync(TemplatesListFilter filter);
    }
}
