using F4ConversationCloud.SuperAdmin.Handler;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace F4ConversationCloud.SuperAdmin.Models
{
    public class TemplateViewModel : IValidatableObject
    {
        [Required(ErrorMessage = "Please enter template name")]
        [RegularExpression("^[a-z0-9_]+$", ErrorMessage = "Template names can only contain small letters, numbers and underscores.")]
        [StringLength(512, ErrorMessage = "Template Name should be less than 512 characters.")]
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
        //public int ButtonCategory { get; set; }
        public IEnumerable<SelectListItem> MarketingTemplateTypeList { get; set; }
        public IEnumerable<SelectListItem> UtilityTemplateTypeList { get; set; }
        public IEnumerable<SelectListItem> AuthenticationTemplateTypeList { get; set; }
        public IEnumerable<SelectListItem> ButtonCategoryList { get; set; }
        public IEnumerable<SelectListItem> CustomButtonTypeList { get; set; }

        [Required(ErrorMessage = "Please select language")]
        public int Language { get; set; }
        public IEnumerable<SelectListItem> LanguageList { get; set; }
        public int VariableType { get; set; }
        public IEnumerable<SelectListItem> VariableTypeList { get; set; }
        public int MediaType { get; set; }
        public IEnumerable<SelectListItem> MediaTypeList { get; set; }
        [MediaFileValidation]
        public IFormFile File { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }

        [StringLength(60, ErrorMessage = "Header should be less than 60 characters.")]
        [HeaderVariableFormat]
        public string Header { get; set; }

        [Required(ErrorMessage = "Please enter a message.")]
        [StringLength(550, ErrorMessage = "Body should be less than 550 characters.")]
        [MessageBodyValidation(ErrorMessage = "Invalid variable formatting.")]
        public string MessageBody { get; set; }
        [StringLength(60, ErrorMessage = "Footer should be less than 60 characters.")]
        public string Footer { get; set; }
        public string HeaderVariableName { get; set; }
        public string HeaderVariableValue { get; set; } = string.Empty;
        public List<BodyVariable> bodyVariables { get; set; } = new List<BodyVariable>();
        public int ClientInfoId { get; set; }
        public int MetaConfigId { get; set; }
        public string WABAId { get; set; }
        public string PageMode { get; set; }
        public string TemplateId { get; set; }
        public int TemplateTableId { get; set; }
        public List<Button> buttons { get; set; } = new List<Button>();

        public class BodyVariable()
        {
            [Required(ErrorMessage = "Please enter a sample for this variable.")]
            public string BodyVariableName { get; set; }
            public string BodyVariableValue { get; set; } = string.Empty;
        }

        public class Button()
        {
            public int ButtonType { get; set; }
            [Required(ErrorMessage = "This field is required")]
            [StringLength(25, ErrorMessage = "Button text should be less than 25 characters.")]
            public string ButtonText { get; set; }
            public int ButtonCategory { get; set; }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Header) && Header.Contains("{{1}}") && string.IsNullOrWhiteSpace(HeaderVariableValue))
            {
                yield return new ValidationResult(
                    "Add sample text.",
                    new[] { nameof(HeaderVariableValue) }
                );
            }

            var normalized = buttons
               .Select((b, i) => new { Index = i, Text = (b?.ButtonText ?? "").Trim() })
               .Where(x => !string.IsNullOrEmpty(x.Text))
               .GroupBy(x => x.Text, StringComparer.OrdinalIgnoreCase)
               .Where(g => g.Count() > 1);

            foreach (var group in normalized)
            {
                // For each duplicate group, mark all but the first as invalid (or all if you prefer)
                bool first = true;
                foreach (var item in group)
                {
                    if (first) { first = false; continue; }
                    // Target the exact field: buttons[i].ButtonText
                    yield return new ValidationResult(
                        $"You can't enter same text for multiple buttons.",
                        new[] { $"buttons[{item.Index}].ButtonText" }
                    );
                }
            }
        }
    }
}
