using F4ConversationCloud.Application.Common.Models.MetaCloudApiModel.Templates;
using F4ConversationCloud.Domain.Entities;
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
        public string Templatename { get; set; }
        public string Templatelanguage { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        [ValidMetaTemplateCategory(ErrorMessage = "The category must be 'Authentication', 'Marketing', or 'Utility'.")]
        public string Templatecategory { get; set; }
        public HeaderComponent TemplateHeader { get; set; }
        public BodyComponent? TemplateBody { get; set; }
        public ButtonsComponent? TemplateButton { get; set; }
        public FooterComponent TemplateFooter { get; set; }

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
    [Required(ErrorMessage = "This is required field.")]
    public string? Format { get; set; }
    public string? Text { get; set; }
    public HeaderNameVariable? NameVariable { get; set; }
    public HeaderNumberVariable? NumberVariable { get; set; }
    public IFormFile? Files { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var validformat = new[] { "TEXT", "IMAGE", "LOCATION", "DOCUMENT" };
        if (Array.IndexOf(validformat, Format) == -1)
        {
            yield return new ValidationResult("Templates format is incorrect.");
        }
        else
        {
            if (Format == "TEXT")
            {
                if (string.IsNullOrEmpty(Text))
                {
                    yield return new ValidationResult("Header text cannot be empty.");

                }
                else
                {
                    if (Text.Contains("{{"))
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
            else if (Format == "IMAGE")
            {
                if (Files == null)
                {
                    yield return new ValidationResult("File required for image template");

                }
            }
            else if (Format == "DOCUMENT")
            {
                if (Files == null)
                {
                    yield return new ValidationResult("File required for documenttemplate");

                }
            }
            else if (Format == "VIDEO")
            {
                if (Files == null)
                {
                    yield return new ValidationResult("File required for documenttemplate");

                }
            }
        }


    }
}

public class BodyComponent 
{
    public BodyNameVariable? BodyNameVariable { get; set; }
    public BodyNumberVariable? BodyNumberVariable { get; set; }

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
    public BodyNameVariable BodyNameVariable { get; set; }
    public BodyNumberVariable BodyNumberVariable { get; set; }
}


public class BodyNameVariable
{

    [RegularExpression(@"^\{\{[a-z][a-z0-9_]{1,50}\}\}$", ErrorMessage = "Variable parameters must start with a lowercase letter and contain only lowercase letters, underscores, and numbers (e.g., {{customer_name}}, {{order_id}}).")]
    public List<string>? text { get; set; }
    public BodyNameVariableExample? BodyNameVariableExample { get; set; }
}

public class BodyNameVariableExample
{
    [RegularExpression(@"^\{\{\d+\}\}$", ErrorMessage = "Variable parameters must be whole numbers with two sets of curly brackets (e.g., {{1}}, {{2}}).")]
    public List<List<string>> body_text { get; set; }
}

public class BodyNumberVariable
{
    public List<string>? body_text { get; set; }

    public BodyNameVariableExample? BodyNumberVariableExample { get; set; }
}

public class ButtonsComponent
{
    public List<Button>? Buttons { get; set; }

}


public class Button 
{
    public string? Type { get; set; }

    public ButtonPhone TemplatePhoneButton { get; set; }

    public ButtonQuickReply TemplateQuickReplyButton { get; set; }

    public ButtonURL TemplateURLButton { get; set; }
}

public class ButtonPhone () : IValidatableObject
{
    public string Text { get; set; }
    public string Type { get; set; }

    [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Please enter a 10-digit phone number.")]
    public string? PhoneNumber { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var validformat = new[] { "PHONE_NUMBER"};
        if (Array.IndexOf(validformat, Type) == -1)
        {
            yield return new ValidationResult("Button type is incorrect.");
        } 
    }
}


public class ButtonURL() : IValidatableObject
{

    public string Text { get; set; }
    public string Type { get; set; }

    [RegularExpression(@"^(https?:\/\/)?[a-z0-9-]+(\.[a-z0-9-]+)+[^\s]*$", ErrorMessage = "Please enter a valid URL.")]
    public string? URL { get; set; }


    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var validformat = new[] { "URL" };
        if (Array.IndexOf(validformat, Type) == -1)
        {
            yield return new ValidationResult("Button type is incorrect.");
        }

    }

}


public class ButtonQuickReply() : IValidatableObject
{

    public string Text { get; set; }
    public string Type { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var validformat = new[] { "QUICK_REPLY" };
        if (Array.IndexOf(validformat, Type) == -1)
        {
            yield return new ValidationResult("Button type is incorrect.");
        }

    }

}

public class FooterComponent
{
    public string? type { get; set; }
    public string? text { get; set; }
}




