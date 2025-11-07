using F4ConversationCloud.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models.CommonModels
{
    public class WhatsappTemplateListFilter
    {

        public int ClientInfoId { get; set; }
        public string TemplateName { get; set; } = string.Empty;
        public TemplateModuleType Category { get; set; }
        public string LangCode { get; set; } = string.Empty;
        public DateTime? ModifiedOn { get; set; }
        public string TemplateStatus { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
