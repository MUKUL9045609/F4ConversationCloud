using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models.CommonModels
{
    public class WhatsappTemplateListItem
    {
        public int SrNo { get; set; }
        public string TemplateId { get; set; }
        public string TemplateName { get; set; }
        public string LanguageCode { get; set; }
        public string Category { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string TemplateStatus{ get; set; }

    }
}
