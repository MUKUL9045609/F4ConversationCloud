using F4ConversationCloud.Application.Common.Models.MetaCloudApiModel.Templates;

namespace F4ConversationCloud.SuperAdmin.Models
{
    public class TemplatesListViewModel
    {
        public string SearchString { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 100;
        public int TotalCount { get; set; } = 100;
        public TemplateListViewItem Columns { get; set; } = new TemplateListViewItem();
       // public List<TemplateData> TemplatesList { get; set; } = new List<TemplateData>();
        public IEnumerable<TemplateData> data { get; set; } = new List<TemplateData>();
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
