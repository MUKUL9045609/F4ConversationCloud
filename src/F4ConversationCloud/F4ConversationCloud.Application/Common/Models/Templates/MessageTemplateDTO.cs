using F4ConversationCloud.Domain.Enum;
using F4ConversationCloud.WebUI.Handler;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models.Templates
{
    public class MessageTemplateDTO 
    {
        [Required(ErrorMessage = "Template name is required.")]
        [RegularExpression(@"^[a-zA-Z0-9_-]{3,50}$", ErrorMessage = "Template name must be between 3 and 50 characters and can only contain letters, numbers, hyphens, and underscores.")]
        [Display(Name = "Template Name")]
        public string? name { get; set; }
        public string? language { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        [ValidMetaTemplateCategory(ErrorMessage = "The category must be 'Authentication', 'Marketing', or 'Utility'.")]
        public string category { get; set; }

        [Required(ErrorMessage = "At least one component is required.")]
        public List<dynamic> components { get; set; } = new List<dynamic>();

    }

    //public class HeaderComponent
    //{
    //    public string? type { get; set; } = null;
    //    public string? format { get; set; } = null;
    //    public string? text { get; set; } = null;
    //    public HeaderExample? example { get; set; } = null;
    //}

    //public class BodyComponent
    //{
    //    public string? type { get; set; } = null;
    //    public string? text { get; set; } = null;
    //    public BodyExample? example { get; set; } = null;
    //}

    //public class FooterComponent
    //{
    //    public string? type { get; set; } = null;
    //    public string? text { get; set; } = null;
    //}

    //public class ButtonComponent
    //{
    //    public string? type { get; set; } = null;
    //    public List<Button>? buttons { get; set; } = null;
    //}

    //public class HeaderExample
    //{
    //    public List<string>? header_text { get; set; } = null;
    //}

    //public class BodyExample
    //{
    //    public List<List<string>>? body_text { get; set; } = null;
    //}

    //public class Button
    //{
    //    public string? type { get; set; } = null;
    //    public string? text { get; set; } = null;
    //}

}
