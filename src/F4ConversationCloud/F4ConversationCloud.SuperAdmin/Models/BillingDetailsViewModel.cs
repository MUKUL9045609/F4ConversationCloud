using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using System.ComponentModel.DataAnnotations;

namespace F4ConversationCloud.SuperAdmin.Models
{
    public class BillingDetailsViewModel
    {
        public string PhoneNumberId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public BillingDetailsViewItem Columns { get; set; } = new BillingDetailsViewItem();
        public IEnumerable<TemplateMessageInsightsListViewItem> data { get; set; } = new List<TemplateMessageInsightsListViewItem>();

        public class BillingDetailsViewItem
        {
            public int Id { get; set; }

            [Display(Name = "Sr. No.")]
            public int SrNo { get; set; }

            [Display(Name = "Duration")]
            public string Duration { get; set; }

            [Display(Name = "Conversation Type")]
            public string ConversationType { get; set; }

            [Display(Name = "Message")]
            public int Message { get; set; }

            [Display(Name = "Amount")]

            public decimal Amount { get; set; }



        }
    }
}
