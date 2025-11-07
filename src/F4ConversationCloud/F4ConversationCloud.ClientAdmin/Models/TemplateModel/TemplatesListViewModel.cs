using F4ConversationCloud.Application.Common.Models.CommonModels;
using F4ConversationCloud.Domain.Enum;

namespace F4ConversationCloud.ClientAdmin.Models.TemplateModel
{
    public class TemplatesListViewModel
    {
        public int SrNo { get; set; }
        public int ClientInfoId { get; set; }

        public string TemplateName { get; set; } = string.Empty;

        public TemplateModuleType Category { get; set; }

        public string LangCode { get; set; } = string.Empty;

        public DateTime? ModifiedOn { get; set; }

        public string TemplateStatus { get; set; } = string.Empty;

       
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
        
        public IEnumerable<WhatsappTemplateListItem> Templates { get; set; }
        public int TotalCount { get; set; }
            

    }
}
