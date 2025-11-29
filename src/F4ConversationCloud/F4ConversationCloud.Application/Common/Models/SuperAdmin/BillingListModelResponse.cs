using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models.SuperAdmin
{
    public class BillingListModelResponse
    {
        public int TotalCount { get; set; }
        public List<BillingListItem> billingListItems { get; set; }
    }
    public class BillingListItem
    {
        public int SrNo { get; set; }
        public string OrganizationsName { get; set; }
        public int ClientInfoId { get; set; }
        public string ClientId { get; set; }
        public string WhatsAppDisplayName { get; set; }
        public string WabaPhoneNumber { get; set; }
        public string phoneNumberId { get; set; }
        public string MetaConfigid { get; set; }
        public List<TemplateMessageInsightsListViewItem> TemplateInsightsList { get; set; }
    }
    public class BillingListFilter
    {
        public string OrganizationsNameFilter { get; set; }
        public string WabaPhoneNumberFilter { get; set; }
        public string WhatsAppDisplayNameFilter { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;


    }
}
