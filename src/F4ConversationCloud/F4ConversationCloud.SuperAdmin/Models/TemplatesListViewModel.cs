namespace F4ConversationCloud.SuperAdmin.Models
{
    public class TemplatesListViewModel
    {
        public string SearchString { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; } = 10;
        public TemplateListViewItem Columns { get; set; } = new TemplateListViewItem();
        public IEnumerable<TemplateListViewItem> data { get; set; } = new List<TemplateListViewItem>();
        public class TemplateListViewItem
        {
            public int Id { get; set; }
            public int SrNo { get; set; }
            public string Name { get; set; }
            public string Language { get; set; }
            public string Category { get; set; }
            public string Status { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime? UpdatedOn { get; set; }
        }
    }
}
