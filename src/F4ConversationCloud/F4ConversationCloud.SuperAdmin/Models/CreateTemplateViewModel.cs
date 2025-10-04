using F4ConversationCloud.Application.Common.Models.MetaCloudApiModel.Templates;
using System.Text.Json.Serialization;

namespace F4ConversationCloud.SuperAdmin.Models
{
    public class CreateTemplateViewModel
    {
        public string TemplateName { get; set; }
        public string Language { get; set; }
        public string Category { get; set; }
        public List<ComponentViewModel> Components { get; set; } = new();
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
        //public List<string> Header_Text { get; set; }
        
        public List<string> HeaderText { get; set; } 
        public List<List<string>> BodyText { get; set; }
    }

    public class ButtonViewModel
    {
        public string Type { get; set; }   
        public string Text { get; set; }
        public string Url { get; set; }    
    }

}
