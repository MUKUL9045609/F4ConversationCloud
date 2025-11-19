using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models.CommonModels
{
    public class TemplateModel
    {
        public int Id { get; set; }
        public int WABAId { get; set; }
        public int SrNo { get; set; }
        public string TemplateName { get; set; }
        public string TemplateCategory { get; set; }
        public string Language { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Status { get; set; }
        public string HeaderText { get; set; }
        public string HeaderExample { get; set; }
        public string BodyText { get; set; }
        public string BodyExample { get; set; }
        public string FooterText { get; set; }
        public int IsActive { get; set; }
        public string HeaderMediaUrl { get; set; }
        public string HeaderMediaName { get; set; }
        public List<Button> buttons { get; set; } = new List<Button>();
        public class Button
        {
            public int WhatsAppTemplateId { get; set; }
            public int ButtonType { get; set; }
            public string ButtonText { get; set; }
            public int ButtonCategory { get; set; }
        }
    }
}
