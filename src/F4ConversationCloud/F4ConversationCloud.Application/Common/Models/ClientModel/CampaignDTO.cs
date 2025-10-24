using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models.ClientModel
{
    public class CampaignDTO
    {
        public string SearchString { get; set; } = string.Empty;
        public bool Status { get; set; } = true;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; } = 0;
        public CampaignListViewItem Columns { get; set; } = new CampaignListViewItem();
        public IEnumerable<CampaignListViewItem> data { get; set; } = Enumerable.Empty<CampaignListViewItem>();

        public class CampaignListViewItem
        {
            [Display(Name = "Sr No")]
            public string SrNo { get; set; } = string.Empty;
            public string Id { get; set; } = string.Empty;

            [Display(Name = "File Name")]
            public string FileName { get; set; } = string.Empty;

            [Display(Name = "FileUrl")]
            public string FileUrl { get; set; } = string.Empty;

            [Display(Name = "From Date")]
            public string FromDate { get; set; } = string.Empty;

            [Display(Name = "To Date")]
            public string ToDate { get; set; } = string.Empty;

            [Display(Name = "Status")]
            public string Status { get; set; } = string.Empty;

            [Display(Name = "Message")]
            public string Message { get; set; } = string.Empty;

            [Display(Name = "Created Date")]
            public DateTime CreatedDate { get; set; }

            [Display(Name = "Modified Date")]
            public DateTime? ModifiedDate { get; set; }
        }
    }
}
