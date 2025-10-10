using F4ConversationCloud.Domain.Enum;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace F4ConversationCloud.SuperAdmin.Handler
{
    public class HeaderVariableFormatAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly string _variableTypeProperty;

        public HeaderVariableFormatAttribute(string variableTypeProperty)
        {
            _variableTypeProperty = variableTypeProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var header = value as string;
            if (string.IsNullOrWhiteSpace(header))
                return ValidationResult.Success;

            var variableTypeProp = validationContext.ObjectType.GetProperty(_variableTypeProperty);
            if (variableTypeProp == null)
                return new ValidationResult($"Unknown property: {_variableTypeProperty}");

            var variableTypeValue = variableTypeProp.GetValue(validationContext.ObjectInstance, null);
            if (variableTypeValue == null)
                return new ValidationResult("Variable type is required.");

            int variableType = (int)variableTypeValue;

            if (header.Contains("{{") || header.Contains("}}") || header.Contains("{}}") || header.Contains("{{}"))
            {
                if (variableType == (int)VariableTypes.Number)
                {
                    var regex = new Regex(@"^\s*\{\{\d+\}\}\s*$");
                    if (!regex.IsMatch(header.Trim()))
                    {
                        return new ValidationResult("Variable parameters must be whole numbers with two sets of curly brackets (e.g., {{1}}, {{2}}).");
                    }
                }
                else if (variableType == (int)VariableTypes.Name)
                {
                    var regex = new Regex(@"^\s*\{\{[a-z0-9_]+\}\}\s*$");
                    if (!regex.IsMatch(header.Trim()))
                    {
                        return new ValidationResult("Variable parameters must be lowercase characters, underscores and numbers with two sets of curly brackets (e.g., {{customer_name}}, {{order_id}}).");
                    }
                }
            }

            return ValidationResult.Success;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-headervariableformat", ErrorMessage ?? "Invalid variable format.");
            context.Attributes.Add("data-val-headervariableformat-variabletype", _variableTypeProperty);
        }
    }
}
