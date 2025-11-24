using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using System.ComponentModel.DataAnnotations;

namespace F4ConversationCloud.SuperAdmin.Models
{
    public class UsageViewModel
    {

        public string OrganizationsNameFilter { get; set; } = string.Empty;
        public string WabaPhoneNumberFilter { get; set; } = string.Empty;
        public string WhatsAppDisplayNameFilter { get; set; } = string.Empty;

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; } = 0;
        public UsageListViewItem Columns { get; set; } = new UsageListViewItem();
        public IEnumerable<UsageModel> data { get; set; } = new List<UsageModel>();
     
        public class UsageListViewItem
        {
            public int Id { get; set; }

            [Display(Name = "Sr. No.")]
            public int SrNo { get; set; }

            [Display(Name = "Organization Name")]
            public string OrganizationName { get; set; }

            [Display(Name = "WhatsApp Display Name")]
            public string WhatsAppDisplayName { get; set; }

            [Display(Name = "Waba Phone Number")]
            public string WabaPhoneNumber { get; set; }

            public DateTime CreatedAt { get; set; }

            public DateTime? UpdatedOn { get; set; }
            [Display(Name = "Client Id")]
            public string? ClientId { get; set; }
        }
    }
}
