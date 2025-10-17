using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace F4ConversationCloud.Application.Common.Models.Templates
{
    public class TemplateRequest
    {
        public int? Id { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_-]{3,50}$", ErrorMessage = "Template name must be 3-50 characters, using letters, numbers, underscores or hyphens.")]
        public string Name { get; set; }

        [Required]
        public string Language { get; set; }

        [Required]
        [RegularExpression("^(AUTHENTICATION|MARKETING|UTILITY)$", ErrorMessage = "Category must be AUTHENTICATION, MARKETING, or UTILITY.")]
        public string Category { get; set; }

        public HeaderComponent TemplateHeader { get; set; } = new HeaderComponent();
        public BodyComponent TemplateBody { get; set; } = new BodyComponent();
        public FooterComponent TemplateFooter { get; set; } = new FooterComponent();
        public ButtonComponet TemplateButton { get; set; } = new ButtonComponet();



    }

    public class Component : IValidatableObject
    {
        [Required(ErrorMessage = "Component type is required.")]
        public string Type { get; set; }

        public string? Format { get; set; }
        public string? Text { get; set; }
        public HeaderExample? Example { get; set; }
        public BodyExample? Body_Example { get; set; }
        public List<Button>? Buttons { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Type))
            {
                yield return new ValidationResult("Component type is missing.");
                yield break;
            }

            switch (Type.ToUpper())
            {
                case "HEADER":
                    var validFormats = new[] { "TEXT", "IMAGE", "DOCUMENT", "LOCATION" };
                    if (string.IsNullOrWhiteSpace(Format) || !validFormats.Contains(Format.ToUpper()))
                    {
                        yield return new ValidationResult("Header format must be one of: TEXT, IMAGE, DOCUMENT, LOCATION.");
                        yield break;
                    }

                    string formatUpper = Format.ToUpper();
                    if (formatUpper == "TEXT" && string.IsNullOrWhiteSpace(Text))
                        yield return new ValidationResult("Header text is required for TEXT format.");

                    if (Example != null)
                    {
                        Example.Format ??= formatUpper;
                        var exampleValidation = new List<ValidationResult>();
                        Validator.TryValidateObject(Example, new ValidationContext(Example), exampleValidation, true);
                        foreach (var result in exampleValidation)
                            yield return result;
                    }
                    break;

                case "BODY":
                    if (string.IsNullOrWhiteSpace(Text))
                        yield return new ValidationResult("Body text is required.");

                    if (Body_Example == null || Body_Example.Body_Text == null || !Body_Example.Body_Text.Any())
                        yield return new ValidationResult("Body example is required.");
                    break;

                case "FOOTER":
                    if (string.IsNullOrWhiteSpace(Text))
                        yield return new ValidationResult("Footer text is required.");
                    break;

                case "BUTTONS":
                    if (Buttons == null || !Buttons.Any())
                    {
                        yield return new ValidationResult("At least one button must be provided.");
                    }
                    else
                    {
                        foreach (var button in Buttons)
                        {
                            var buttonValidation = new List<ValidationResult>();
                            Validator.TryValidateObject(button, new ValidationContext(button), buttonValidation, true);
                            foreach (var result in buttonValidation)
                                yield return result;
                        }
                    }
                    break;

                default:
                    yield return new ValidationResult($"Unknown component type: {Type}");
                    break;
            }
        }
    }
    public class HeaderComponent : IValidatableObject
    {
        [Required(ErrorMessage = "Component type is required.")]
        public string Type { get; set; }

        public string? Format { get; set; }
        public string? Text { get; set; }
        public HeaderExample? Example { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Type))
            {
                yield return new ValidationResult("Component type is missing.");
                yield break;
            }

            switch (Type.ToUpper())
            {
                case "HEADER":
                    var validFormats = new[] { "TEXT", "IMAGE", "DOCUMENT", "LOCATION" };
                    if (string.IsNullOrWhiteSpace(Format) || !validFormats.Contains(Format.ToUpper()))
                    {
                        yield return new ValidationResult("Header format must be one of: TEXT, IMAGE, DOCUMENT, LOCATION.");
                        yield break;
                    }

                    string formatUpper = Format.ToUpper();
                    if (formatUpper == "TEXT" && string.IsNullOrWhiteSpace(Text))
                        yield return new ValidationResult("Header text is required for TEXT format.");

                    if (Example != null)
                    {
                        Example.Format ??= formatUpper;
                        var exampleValidation = new List<ValidationResult>();
                        Validator.TryValidateObject(Example, new ValidationContext(Example), exampleValidation, true);
                        foreach (var result in exampleValidation)
                            yield return result;
                    }
                    break;
                default:
                    yield return new ValidationResult($"Unknown component type: {Type}");
                    break;
            }
        }
    }
    public class BodyComponent : IValidatableObject
    {
        [Required(ErrorMessage = "Component type is required.")]
        public string Type { get; set; }

        public string? Format { get; set; }
        public string? Text { get; set; }
        public BodyExample? Body_Example { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Type))
            {
                yield return new ValidationResult("Component type is missing.");
                yield break;
            }

            switch (Type.ToUpper())
            {
                case "BODY":
                    if (string.IsNullOrWhiteSpace(Text))
                        yield return new ValidationResult("Body text is required.");

                    if (Body_Example == null || Body_Example.Body_Text == null || !Body_Example.Body_Text.Any())
                        yield return new ValidationResult("Body example is required.");
                    break;

                default:
                    yield return new ValidationResult($"Unknown component type: {Type}");
                    break;
            }
        }
    }
    public class ButtonComponet : IValidatableObject
    {
        public string? Type { get; set; }

        public string? Format { get; set; }
        public string? Text { get; set; }
        public List<Button>? Buttons { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //if (string.IsNullOrWhiteSpace(Type))
            //{
            //    yield return new ValidationResult("Component type is missing.");
            //    yield break;
            //}
            if (!string.IsNullOrEmpty(Type))
             {

                switch (Type.ToUpper())
                {
                    case "BUTTONS":
                        if (Buttons == null || !Buttons.Any())
                        {
                            yield return new ValidationResult("At least one button must be provided.");
                        }
                        else
                        {
                            foreach (var button in Buttons)
                            {
                                var buttonValidation = new List<ValidationResult>();
                                Validator.TryValidateObject(button, new ValidationContext(button), buttonValidation, true);
                                foreach (var result in buttonValidation)
                                    yield return result;
                            }
                        }
                        break;

                    default:
                        yield return new ValidationResult($"Unknown component type: {Type}");
                        break;
                }
            }
        }
    }
    public class FooterComponet : IValidatableObject
    {
        [Required(ErrorMessage = "Component type is required.")]
        public string Type { get; set; }

        public string? Format { get; set; }
        public string? Text { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Type))
            {
                yield return new ValidationResult("Component type is missing.");
                yield break;
            }

            switch (Type.ToUpper())
            {
                case "FOOTER":
                    if (string.IsNullOrWhiteSpace(Text))
                        yield return new ValidationResult("Footer text is required.");
                    break;
                default:
                    yield return new ValidationResult($"Unknown component type: {Type}");
                    break;
            }
        }
    }


    //BodyExamples
    public class HeaderExample : IValidatableObject
    {
        public List<string>? Header_Text { get; set; }
        public string? HeaderFile { get; set; }
        public string? Format { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Format))
            {
                yield return new ValidationResult("Header format is required to validate HeaderExample.");
                yield break;
            }

            var format = Format.ToUpper();
            switch (format)
            {
                case "TEXT":
                    if (Header_Text == null || !Header_Text.Any())
                        yield return new ValidationResult("Header_Text is required for TEXT format.");
                    break;
                case "IMAGE":
                case "DOCUMENT":
                    if (string.IsNullOrWhiteSpace(HeaderFile))
                        yield return new ValidationResult($"HeaderFile is required for {format} format.");
                    break;
                case "LOCATION":
                    break;
                default:
                    yield return new ValidationResult("Unsupported header format.");
                    break;
            }
        }
    }

    public class BodyExample : IValidatableObject
    {
        public List<List<string>>? Body_Text { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Body_Text == null || !Body_Text.Any())
            {
                yield return new ValidationResult("Body_Text is required and cannot be empty.");
                yield break;
            }

            for (int i = 0; i < Body_Text.Count; i++)
            {
                if (Body_Text[i] == null || !Body_Text[i].Any())
                    yield return new ValidationResult($"Body_Text row {i + 1} must contain at least one value.");
            }
        }
    }

    public class Button : IValidatableObject
    {
        [Required]
        public string Type { get; set; }
        [Required]
        public string Text { get; set; }
        public string? Phone_Number { get; set; }
        public string? Url { get; set; }
        public List<string>? Example { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            switch (Type?.ToUpper())
            {
                case "QUICK_REPLY":
                    break;
                case "PHONE_NUMBER":
                    if (string.IsNullOrWhiteSpace(Phone_Number))
                        yield return new ValidationResult("Phone number is required for PHONE_NUMBER button.");
                    break;
                case "URL":
                    if (string.IsNullOrWhiteSpace(Url))
                        yield return new ValidationResult("URL is required for URL button.");
                    break;
                default:
                    yield return new ValidationResult($"Unsupported button type: {Type}");
                    break;
            }
        }
    }
}
