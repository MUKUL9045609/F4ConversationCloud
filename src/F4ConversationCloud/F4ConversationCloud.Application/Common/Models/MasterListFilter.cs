namespace F4ConversationCloud.Application.Common.Models
{
    public class MasterListFilter
    {
        public string SearchString { get; set; } = string.Empty;
        public bool Status { get; set; } = true;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
