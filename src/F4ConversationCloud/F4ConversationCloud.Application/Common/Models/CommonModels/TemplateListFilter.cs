using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models.CommonModels
{
    public class TemplateListFilter
    {
        public string WABAId { get; set; } = string.Empty;
        public string TemplateNameFilter { get; set; } = string.Empty;
        public int TemplateCategoryFilter { get; set; } = 0;
        public int LanguageFilter { get; set; } = 0;
        public string CreatedOnFilter { get; set; } = string.Empty;
        public int StatusFilter { get; set; } = 0;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
