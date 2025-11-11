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
        public string TemplateName { get; set; }
        public TemplateModuleType? Category { get; set; }   
        public TemplateLanguages? LangCode { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public TemplateApprovalStatus? TemplateStatus { get; set; } 
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
