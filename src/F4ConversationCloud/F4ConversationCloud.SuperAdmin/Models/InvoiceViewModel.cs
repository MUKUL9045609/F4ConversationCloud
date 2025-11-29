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

        public string LogoBase64 { get; set; }

        public InvoiceViewItem InvoiceData { get; set; }
        public DateTime InvoiceDate { get; set; } = DateTime.Now;

        public IEnumerable<TemplateMessageInsightsListViewItem> TemplateMessageInsights { get; set; }

        public decimal SubTotal => TemplateMessageInsights?.Sum(x => x.TotalAmount) ?? 0;

        public decimal TotalCGST => TemplateMessageInsights?.Sum(x => x.TotalAmount * x.CGSTTaxs / 100) ?? 0;
        public decimal TotalSGST => TemplateMessageInsights?.Sum(x => x.TotalAmount * x.SGSTTaxs / 100) ?? 0;
        public decimal TotalIGST => TemplateMessageInsights?.Sum(x => x.TotalAmount * x.IGSTTaxs / 100) ?? 0;

        public decimal TotalTax => TotalCGST + TotalSGST + TotalIGST;

        public decimal GrandTotal => SubTotal + TotalTax;
    }

}
