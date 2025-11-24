using System.ComponentModel.DataAnnotations;

namespace F4ConversationCloud.SuperAdmin.Models
{
    public class BusinessAccountListViewModel
    {
        public string OrganizationsFilter { get; set; } = string.Empty;
        public string OnboardingOnFilter { get; set; } = string.Empty;
        public string PhoneNumberFilter { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; } = 0;
        public BusinessAccountListItem Columns { get; set; } = new BusinessAccountListItem();
        public IEnumerable<BusinessAccountListItem> data { get; set; } = new List<BusinessAccountListItem>();
        public class BusinessAccountListItem
        {
            public int Id { get; set; }

            [Display(Name = "Sr. No.")]
            public int SrNo { get; set; }

            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Organizations")]
            public string Organizations { get; set; }

            [Display(Name = "Onboarding Date")]
            public DateTime CreatedAt { get; set; }

            public DateTime? UpdatedOn { get; set; }

            [Display(Name = "Client Id")]
            public string ClientId { get; set; }

            [Display(Name = "WABA Accounts")]
            public int ClientWaBaAccCount { get; set; }
        }
    }
}
