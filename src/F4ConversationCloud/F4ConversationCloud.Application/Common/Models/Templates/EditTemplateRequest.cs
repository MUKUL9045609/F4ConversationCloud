using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models.Templates
{
    public class EditTemplateRequest
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

        [Required]
        public string WABAID { get; set; }

        [Required]
        public string ClientInfoId { get; set; }
        public string CreatedBy { get; set; }
        public string TemplateId { get; set; }
    }
}
