using F4ConversationCloud.Application.Common.Models.CommonModels;
using F4ConversationCloud.Application.Common.Models.MetaCloudApiModel.Templates;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace F4ConversationCloud.SuperAdmin.Models
{
    public class TemplatesListViewModel
    {
        public string TemplateNameFilter { get; set; } = string.Empty;
        public int TemplateCategoryFilter { get; set; } = 0;
        public int LanguageFilter { get; set; } = 0;
        public string CreatedOnFilter { get; set; } = string.Empty;
        public int IsActivate { get; set; } = 1;
        public int StatusFilter { get; set; } = 0;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; } = 0;
        public TemplateListViewItem Columns { get; set; } = new TemplateListViewItem();
        public IEnumerable<TemplateListViewItem> data { get; set; } = new List<TemplateListViewItem>();
        public IEnumerable<SelectListItem> StatusList { get; set; }
        public IEnumerable<SelectListItem> TemplateCategoryList { get; set; }
        public IEnumerable<SelectListItem> LanguageList { get; set; }
        public class TemplateListViewItem
        {
            public int Id { get; set; }

            [Display(Name = "Sr. No.")]
            public int SrNo { get; set; }

            [Display(Name = "Template Name")]
            public string TemplateName { get; set; }

            [Display(Name = "Template Category")]
            public string TemplateCategory { get; set; }

            [Display(Name = "Language")]
            public string Language { get; set; }

            [Display(Name = "Created Date")]
            public DateTime CreatedOn { get; set; }

            [Display(Name = "Status")]
            public string Status { get; set; }

            
            public string HeaderText { get; set; }
            public string HeaderExample { get; set; }
            public string BodyText { get; set; }
            public string BodyExample { get; set; }
            public string FooterText { get; set; }
            public int MyProperty { get; set; }
            public int IsActive { get; set; }
            public string HeaderMediaUrl { get; set; }
            public string HeaderMediaName { get; set; }
        }
    }
}
