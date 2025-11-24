using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace F4ConversationCloud.Application.Common.Models.Templates
{
    public class SendTemplateDTO
    {
        [Required]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Please enter a 10-digit phone number.")]
        public string SendTo { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_-]{3,50}$", ErrorMessage = "Template name must be between 3 and 50 characters and can only contain letters, numbers, hyphens, and underscores.")]
        public string TemplateName { get; set; }

        [Required]
        public Dictionary<string, string> TemplateValues { get; set; }

        [Required]
        public string TemplateUrl { get; set; }

        [Required]
        public string PhoneId { get; set; }

    }
}
