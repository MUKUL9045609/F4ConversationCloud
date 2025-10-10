using F4ConversationCloud.SuperAdmin.Handler;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace F4ConversationCloud.SuperAdmin.Models
{
    public class TemplateViewModel
    {
        [Required(ErrorMessage = "Please enter template name")]
        [RegularExpression("^[a-z0-9_]+$", ErrorMessage = "Template names can only contain small letters, numbers and underscores.")]
        public string TemplateName { get; set; }

        [Required(ErrorMessage = "Please select template category")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Please select template category")]
        public int TemplateCategory { get; set; }
        public string TemplateCategoryName { get; set; }
        public IEnumerable<SelectListItem> TemplateCategoryList { get; set; }

        [Required(ErrorMessage = "Please select template type")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Please select template type")]
        public int TemplateType { get; set; }
        public string TemplateTypeName { get; set; }
        public IEnumerable<SelectListItem> MarketingTemplateTypeList { get; set; }
        public IEnumerable<SelectListItem> UtilityTemplateTypeList { get; set; }
        public IEnumerable<SelectListItem> AuthenticationTemplateTypeList { get; set; }

        [Required(ErrorMessage = "Please select language")]
        public int Language { get; set; }
        public IEnumerable<SelectListItem> LanguageList { get; set; }

        [Required(ErrorMessage = "Please select variable type")]
        public int VariableType { get; set; }
        public IEnumerable<SelectListItem> VariableTypeList { get; set; }
        public int MediaType { get; set; }
        public IEnumerable<SelectListItem> MediaTypeList { get; set; }
        [MediaFileValidation]
        public IFormFile File { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }

        [StringLength(60, ErrorMessage = "Header should be less than 60 characters.")]
        public string Header { get; set; }

        [Required(ErrorMessage = "Please enter a message.")]
        [StringLength(550, ErrorMessage = "Body should be less than 550 characters.")]
        public string MessageBody { get; set; }
        [StringLength(60, ErrorMessage = "Footer should be less than 60 characters.")]
        public string Footer { get; set; }

        public CreateTemplateViewModel CreateTemplateViewModel { get; set; } = new();
    }
}
