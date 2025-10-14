using F4ConversationCloud.Application.Common.Models.MetaModel.Configurations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Interfaces.Services.Meta
{
    public interface IMetaConfigurationService
    {
        void AddWhatsAppBusinessCloudApiService( WhatsAppBusinessCloudApiConfig whatsAppConfig, string graphAPIVersion = null);
    }
}
