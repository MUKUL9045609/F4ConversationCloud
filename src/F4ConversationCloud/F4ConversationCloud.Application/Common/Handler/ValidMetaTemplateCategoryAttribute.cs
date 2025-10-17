using BuldanaUrban.Domain.Helpers;
using F4ConversationCloud.Domain.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace F4ConversationCloud.WebUI.Handler
{
    public class ValidMetaTemplateCategoryAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            var category = value.ToString();
            if (Enum.GetNames(typeof(TemplateModuleType)).Contains(category))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage ?? $"The category '{category}' is not valid.");
        }
    }
}
