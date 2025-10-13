using F4ConversationCloud.Domain.Enum;
using F4ConversationCloud.WebUI.Handler;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace F4ConversationCloud.Application.Common.Models.Templates
{
    public class MessageTemplate : IValidatableObject
    {
        [Required(ErrorMessage = "Template name is required.")]
        [RegularExpression(@"^[a-zA-Z0-9_-]{3,50}$", ErrorMessage = "Template name must be between 3 and 50 characters and can only contain letters, numbers, hyphens, and underscores.")]
        [Display(Name = "Template Name")]
        public string? Templatename { get; set; }
        public string? Templatelanguage { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        [ValidMetaTemplateCategory(ErrorMessage = "The category must be 'Authentication', 'Marketing', or 'Utility'.")]
        public string Templatecategory { get; set; }
        public List<dynamic> components { get; set; } = new List<dynamic>();
        public HeaderComponent TemplateHeader { get; set; }
        public BodyComponent TemplateBody { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Enum.GetNames(typeof(TemplateLanguages)).Contains(Templatelanguage))
            {
                yield return new ValidationResult("Templates language is incorrect.");
            }
        }
    }
}

public class HeaderComponent : IValidatableObject
{
    [Required(ErrorMessage = "This is required.")]
    public string? format { get; set; }
    public string? text { get; set; }
    public HeaderNameVariable? NameVariable { get; set; }
    public HeaderNumberVariable? NumberVariable { get; set; }
    public IFormFile? Files { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var validformat = new[] { "TEXT", "IMAGE", "LOCATION", "DOCUMENT" };
        if (Array.IndexOf(validformat, format) == -1)
        {
            yield return new ValidationResult("Templates format is incorrect.");
        }
        else
        {
            if (format == "TEXT")
            {
                if (string.IsNullOrEmpty(text))
                {
                    yield return new ValidationResult("Header text cannot be empty.");

                }
                else
                {
                    if (text.Contains("{{"))
                    {
                        if (NameVariable == null)
                        {
                            yield return new ValidationResult("Enter valid text parameter value.");
                        }
                        else if (NameVariable.header_text == null)
                        {
                            yield return new ValidationResult("Enter valid text parameter value.");
                        }

                    }
                }
            }
            else if (format == "IMAGE")
            {
                if (Files == null)
                {
                    yield return new ValidationResult("File required for image template");

                }
            }
            else if (format == "DOCUMENT")
            {
                if (Files == null)
                {
                    yield return new ValidationResult("File required for documenttemplate");

                }
            }
            else if (format == "VIDEO")
            {
                if (Files == null)
                {
                    yield return new ValidationResult("File required for documenttemplate");

                }
            }
        }


    }
}

public class BodyComponent : IValidatableObject
{
    public string? text { get; set; }
    public BodyExample? example { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrEmpty(text))
        {
            yield return new ValidationResult("Header text cannot be empty.");

        }
        else
        {
            if (text.Contains("{{"))
            {
                //if (BodyNameVariable == null)
                //{
                //    yield return new ValidationResult("Enter valid text parameter value.");
                //}
                //else if (NameVariable.header_text == null)
                //{
                //    yield return new ValidationResult("Enter valid text parameter value.");
                //}

            }
        }
    }
}

public class FooterComponent
{
    public string? type { get; set; }
    public string? text { get; set; }
}

public class ButtonComponent
{
    public string? type { get; set; }
    public List<Button>? buttons { get; set; }
}

public class HeaderNameVariable
{
    [RegularExpression(@"^\{\{[a-z][a-z0-9_]{1,50}\}\}$", ErrorMessage = "Variable parameters must start with a lowercase letter and contain only lowercase letters, underscores, and numbers (e.g., {{customer_name}}, {{order_id}}).")]
    public List<string>? header_text { get; set; }
    public List<string>? header_handle { get; set; }
}

public class HeaderNumberVariable
{
    [RegularExpression(@"^\{\{1\}\}$", ErrorMessage = "Variable parameters must be whole numbers with two sets of curly brackets (e.g., {{1}}).")]
    public List<string>? header_text { get; set; }
    public List<string>? header_handle { get; set; }
}

public class BodyExample
{
    public List<List<string>>? body_text { get; set; }

    public BodyNameVariable BodyNameVariable { get; set; }

    public BodyNumberVariable BodyNumberVariable { get; set; }

}


public class BodyNameVariable
{

    [RegularExpression(@"^\{\{[a-z][a-z0-9_]{1,50}\}\}$", ErrorMessage = "Variable parameters must start with a lowercase letter and contain only lowercase letters, underscores, and numbers (e.g., {{customer_name}}, {{order_id}}).")]
    public List<string>? body_text { get; set; }
}

public class BodyNumberVariable
{
    [RegularExpression(@"^\{\{1\}\}$", ErrorMessage = "Variable parameters must be whole numbers with two sets of curly brackets (e.g., {{1}}).")]
    public List<string>? body_text { get; set; }
}

public class Button
{
    public string? type { get; set; }
    public string? text { get; set; }
}



