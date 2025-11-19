using F4ConversationCloud.Application.Common.Models.CommonModels;
using F4ConversationCloud.Domain.Enum;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace F4ConversationCloud.ClientAdmin.Models.TemplateModel
{
    public class TemplatesListViewModel
    {
        public int SrNo { get; set; }
        public int ClientInfoId { get; set; }

        public string TemplateName { get; set; }

        public TemplateModuleType? Category { get; set; }

        public TemplateLanguages? templateLanguages { get; set; } 

        public DateTime? ModifiedOn { get; set; }

        public TemplateApprovalStatus? TemplateStatus { get; set; }


        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public IEnumerable<SelectListItem> StatusList { get; set; }
        public IEnumerable<SelectListItem> LanguageList { get; set; }

        public IEnumerable<WhatsappTemplateListItem> Templates { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<TemplatesButtonsListItem> TemplateButtons { get; set; }

        public WhatsappTemplateDetail templateDetail { get; set; }


    }
}
