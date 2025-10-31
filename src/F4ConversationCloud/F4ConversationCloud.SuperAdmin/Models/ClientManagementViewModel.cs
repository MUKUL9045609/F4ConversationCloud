using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace F4ConversationCloud.SuperAdmin.Models
{
    public class ClientManagementViewModel
    {
        public string ClientNameSearch { get; set; } = string.Empty;
        public string OnboardingOnFilter { get; set; } = string.Empty;
        public string StatusFilter { get; set; } = string.Empty;
        public string PhoneNumberFilter { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; } = 0;
        public ClientManagementListViewItem Columns { get; set; } = new ClientManagementListViewItem();
        public IEnumerable<ClientManagementListViewItem> data { get; set; } = new List<ClientManagementListViewItem>();
        public int RegistrationId { get; set; } = 0;
        public class ClientManagementListViewItem
        {
            public int Id { get; set; }

            [Display(Name = "Sr. No.")]
            public int SrNo { get; set; }

            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "WhatsApp Display Name")]
            public string ClientName { get; set; }

            [Display(Name = "WhatsApp Account Status")]
            public string Status { get; set; }
            public string Category { get; set; }
            public bool IsActive { get; set; }

            [Display(Name = "Onboarding Date")]
            public DateTime CreatedAt { get; set; }

            public DateTime? UpdatedOn { get; set; }
            [Display(Name = "Client Id")]
            public string? ClientId { get; set; }
        }
    }
}
