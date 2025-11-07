using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models.CommonModels
{
    public class WhatsappTemplateList
    {
        public string TemplateId { get; set; }
        public string TemplateName { get; set; }
        public string LanguageCode { get; set; }
        public string Category { get; set; }
        public string HeaderFormat { get; set; }
        public string BodyText { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedOn { get; set; }

        public string IsActive { get; set; }

    }
}
