using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using System.ComponentModel.DataAnnotations;

namespace F4ConversationCloud.SuperAdmin.Models
{
    public class InvoiceViewModel
    {
        public string PhoneNumberId { get; set; }
        public string MetaConfigid { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public InvoiceViewItem InvoiceData { get; set; }
        public IEnumerable<TemplateMessageInsightsListViewItem> TemplateMessageInsights { get; set; }

    }
}
