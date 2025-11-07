using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models.CommonModels
{
    public class WhatsappTemplateListFilter
    {
       
        public string SearchText { get; set; } = string.Empty;
        public string LanguageCode { get; set; } = string.Empty;
        public string Status { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; } 
    }
}
