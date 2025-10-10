using F4ConversationCloud.Application.Common.Models.MetaCloudApiModel.Templates;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace F4ConversationCloud.SuperAdmin.Models
{
    public class CreateTemplateViewModel : IValidatableObject
    {
        public string TemplateName { get; set; }
        public string Language { get; set; }
        public string Category { get; set; }
        public List<ComponentViewModel> Components { get; set; } = new();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(TemplateName))
            {
                yield return new ValidationResult("Template name is required.", new[] { nameof(TemplateName) });
            }

            var componentValidation = new templatesComponetvalidation(0);
            var result = componentValidation.GetValidationResult(Components, validationContext);
            if (result != ValidationResult.Success)
            {
                yield return result;
            }
        }
    }
    public class ComponentViewModel
    {
        public string Type { get; set; }
        public string Format { get; set; }
        public string Text { get; set; }
        public ExampleViewModel Example { get; set; }
        public List<ButtonViewModel> Buttons { get; set; }
    }

    public class ExampleViewModel
    {
        public List<string> HeaderText { get; set; }
        public List<List<string>> BodyText { get; set; }
    }

    public class ButtonViewModel
    {
        public string Type { get; set; }
        public string Text { get; set; }
        public string Url { get; set; }
    }

    public class templatesComponetvalidation : ValidationAttribute
    {
        private readonly int _type;
        private readonly bool _allowNull;
        public templatesComponetvalidation(int type, bool allowNull = false)
        {
            _type = type;
            _allowNull = allowNull;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var component = (List<ComponentViewModel>)value;
            if (_allowNull && (component == null || component.Count == 0))
            {
                return ValidationResult.Success;
            }
            
            foreach (var comp in component)
            {
                if (string.IsNullOrWhiteSpace(comp.Type))
                {
                    return new ValidationResult("Component type is required.");
                }
                if (comp.Type == "HEADER")
                {
                    //if (string.IsNullOrWhiteSpace(comp.Format))
                    //{
                    //    return new ValidationResult("Header format is required.");
                    //}
                    //if (comp.Format == "TEXT" && string.IsNullOrWhiteSpace(comp.Text))
                    //{
                    //    return new ValidationResult("Header text is required for TEXT format.");
                    //}
                    //if (comp.Format == "IMAGE" && string.IsNullOrWhiteSpace(comp.Text))
                    //{
                    //    return new ValidationResult("Header text is required for IMAGE format.");
                    //}
                    //if (comp.Format == "DOCUMENT" && string.IsNullOrWhiteSpace(comp.Text))
                    //{
                    //    return new ValidationResult("Header text is required for DOCUMENT format.");
                    //}
                    //if (comp.Format == "VIDEO" && string.IsNullOrWhiteSpace(comp.Text))
                    //{
                    //    return new ValidationResult("Header text is required for VIDEO format.");
                    //}
                }
                else if (comp.Type == "BODY")
                {
                    if (string.IsNullOrWhiteSpace(comp.Text))
                    {
                        return new ValidationResult("Body text is required.");
                    }
                }
                else if (comp.Type == "FOOTER")
                {
                    //if (string.IsNullOrWhiteSpace(comp.Text))
                    //{
                    //    return new ValidationResult("Footer text is required.");
                    //}
                }
                else if (comp.Type == "BUTTONS")
                {
                    //if (comp.Buttons == null || comp.Buttons.Count == 0)
                    //{
                    //    return new ValidationResult("At least one button is required.");
                    //}
                    //foreach (var button in comp.Buttons)
                    //{
                    //    if (string.IsNullOrWhiteSpace(button.Type))
                    //    {
                    //        return new ValidationResult("Button type is required.");
                    //    }
                    //    if (string.IsNullOrWhiteSpace(button.Text))
                    //    {
                           


                    //    }
                    //}
                }
            }

            return ValidationResult.Success;
        }
    }
}           
       