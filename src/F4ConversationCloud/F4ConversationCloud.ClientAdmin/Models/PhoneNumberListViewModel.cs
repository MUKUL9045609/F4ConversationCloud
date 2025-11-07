using System.ComponentModel.DataAnnotations;

namespace F4ConversationCloud.ClientAdmin.Models
{
    public class PhoneNumberListViewModel
    {
        public string WABAIdFilter { get; set; } = string.Empty;
        public string BusinessIdFilter { get; set; } = string.Empty;
        public string BusinessCategoryFilter { get; set; } = string.Empty;
        public string WhatsappDisplayNameFilter { get; set; } = string.Empty;
        public string PhoneNumberIdFilter { get; set; } = string.Empty;
        public string PhoneNumberFilter { get; set; } = string.Empty;
        public string StatusFilter { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; } = 0;
        public PhoneNumberListViewItem Columns { get; set; } = new PhoneNumberListViewItem();
        public IEnumerable<PhoneNumberListViewItem> data { get; set; } = new List<PhoneNumberListViewItem>();
        public class PhoneNumberListViewItem
        {
            [Display(Name = "Sr. No.")]
            public int SrNo { get; set; }

            [Display(Name = "WABA Id")]
            public string WABAId { get; set; }

            [Display(Name = "Business Id")]
            public string BusinessId { get; set; }

            [Display(Name = "Business Category")]
            public string BusinessCategory { get; set; }

            [Display(Name = "WhatsApp Display Name")]
            public string WhatsAppDisplayName { get; set; }

            [Display(Name = "Phone Number Id")]
            public string PhoneNumberId { get; set; }

            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Status")]
            public string Status { get; set; }
        }
    }
}
