using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Interfaces.Services
{
    public interface IWhatsAppTemplateParser
    {
        WhatsAppTemplateData Parse(MessageTemplateDTO request);
    }
}
