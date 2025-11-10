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

            if (string.IsNullOrWhiteSpace(header) || header.Trim() == "{}" || header.Trim() == "{" || header.Trim() == "}")
            {
                return ValidationResult.Success;
            }

            // Reject malformed patterns
            if (header.Contains("{{}}") || header.Contains("{{}") || header.Contains("{}}"))
            {
                if (variableType == (int)VariableTypes.Number)
                {
                    return new ValidationResult("Variable parameters must be whole numbers with two sets of curly brackets (e.g., {{1}}).");
                }
                //else if (variableType == (int)VariableTypes.Name)
                //{
                //    return new ValidationResult("Variable parameters must start with a lowercase letter and contain only lowercase letters, underscores, and numbers (e.g., {{customer_name}}, {{order_id}}).");
                //}
            }
            
            if (variableType == (int)VariableTypes.Number)
            {
                var matches = Regex.Matches(header, @"\{\{\d+\}\}");

                // Must contain exactly one valid match and it must be {{1}}
                if (matches.Count != 1 || matches[0].Value != "{{1}}")
                {
                    return new ValidationResult("Variable parameters must be whole numbers with two sets of curly brackets (e.g., {{1}}).");
                }
            }
            //else if (variableType == (int)VariableTypes.Name)
            //{
            //    var matches = Regex.Matches(header, @"\{\{[a-z][a-z0-9_]*\}\}");

            //    // Must contain exactly one valid match and match the full trimmed header
            //    if (matches.Count != 1 || header.Trim() != matches[0].Value)
            //    {
            //        return new ValidationResult("Variable parameters must start with a lowercase letter and contain only lowercase letters, underscores, and numbers (e.g., {{customer_name}}, {{order_id}}).");
            //    }
            //}

            return ValidationResult.Success;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-headervariableformat", ErrorMessage ?? "Variable parameters must be lowercase characters, underscores and numbers with two sets of curly brackets (e.g., {{customer_name}}, {{order_id}}).");
            context.Attributes.Add("data-val-headervariableformat-variabletype", _variableTypeProperty);
        }
    }
}
