namespace F4ConversationCloud.SuperAdmin.Models
{
    public class CreateTemplateViewModel
    {
        public string TemplateName { get; set; }
        public string Language { get; set; }
        public string Category { get; set; }
        public string ComponentType { get; set; }
        public string Text { get; set; }
        public string ButtonType { get; set; }
        public string ButtonText { get; set; }
        public string ButtonUrl { get; set; }
        public string ButtonPayload { get; set; }
    }
}
