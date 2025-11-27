using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models.SuperAdmin
{
    public class InvoiceViewItem
    {
        public string WhatsAppDisplayName { get; set; }
        public string WaBaNumber { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string OrganizationsName { get; set; }
    }

    public class InvoiceResponse
    {
        public IEnumerable<TemplateMessageInsightsListViewItem> TemplateMessageInsights { get; set; }
        public InvoiceViewItem InvoiceDetails { get; set; } 

    }
}
