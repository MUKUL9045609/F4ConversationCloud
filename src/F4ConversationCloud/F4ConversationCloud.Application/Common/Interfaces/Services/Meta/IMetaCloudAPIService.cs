using F4ConversationCloud.Application.Common.Models.MetaCloudApiModel.Response;
using F4ConversationCloud.Application.Common.Models.MetaModel.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Interfaces.Services.Meta
{
    public interface IMetaCloudAPIService
    {
        void SetWhatsAppBusinessConfig(WhatsAppBusinessCloudApiConfig cloudApiConfig);
        Task<TemplateMessageCreationResponse> CreateTemplateMessageAsync(string whatsAppBusinessAccountId, object template, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

    }
}
