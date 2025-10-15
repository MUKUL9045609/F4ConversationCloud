using F4ConversationCloud.Application.Common.Models.MetaCloudApiModel.Templates;

namespace F4ConversationCloud.SuperAdmin.Models
{
    public class TemplatesListViewModel
    {
        public string SearchString { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 100;
        public int TotalCount { get; set; } = 100;
        
        public IEnumerable<TemplateData> data { get; set; } = new List<TemplateData>();

    }
}
