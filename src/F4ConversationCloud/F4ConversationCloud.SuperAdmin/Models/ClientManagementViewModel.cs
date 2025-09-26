using System.ComponentModel.DataAnnotations;

namespace F4ConversationCloud.SuperAdmin.Models
{
    public class ClientManagementViewModel
    {
        public string ClientNameSearch { get; set; } = string.Empty;
        public string OnboardingOnFilter { get; set; } = string.Empty;
        public string StatusFilter { get; set; } = string.Empty;
        public string ApprovalStatusFilter { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; } = 0;
        public ClientManagementListViewItem Columns { get; set; } = new ClientManagementListViewItem();
        public IEnumerable<ClientManagementListViewItem> data { get; set; } = new List<ClientManagementListViewItem>();

        public class ClientManagementListViewItem
        {
            public int Id { get; set; }

            [Display(Name = "Sr. No.")]
            public int SrNo { get; set; }

            [Display(Name = "Client Name")]
            public string ClientName { get; set; }

            [Display(Name = "Status")]
            public string Status { get; set; }

            [Display(Name = "Approval Status")]
            public string ApprovalStatus { get; set; }
            public string Category { get; set; }
            public bool IsActive { get; set; }

            [Display(Name = "Onboarding On")]
            public DateTime CreatedAt { get; set; }

            public DateTime? UpdatedOn { get; set; }
        }
    }
}
