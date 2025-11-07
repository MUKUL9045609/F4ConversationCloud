using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models.CommonModels
{
    public class WhatsAppTemplateResponse
    {
        public IEnumerable<WhatsappTemplateListItem> Templates { get; set; }
        public int TotalCount { get; set; }
    }
}
