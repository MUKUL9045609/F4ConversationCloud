namespace F4ConversationCloud.ClientAdmin.Models.TemplateModel
{
    public class TemplatesListViewModel
    {
        public string SearchText { get; set; } = string.Empty;
        public string LanguageCode { get; set; } = string.Empty;
        public string Status { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
