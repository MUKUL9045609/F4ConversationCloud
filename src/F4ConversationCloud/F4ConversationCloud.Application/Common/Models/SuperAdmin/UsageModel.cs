using F4ConversationCloud.Application.Common.Models.MetaCloudApiModel.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models.SuperAdmin
{
    public class UsageModel
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
    public class UsageModelResponse
    {
        public int TotalCount { get; set; }
        public List<UsageModel>  usageModelsItems { get; set; }

    }
    
    public class TemplateMessageInsightsListViewItem
    {
        public int Id { get; set; }
        public int SrNo { get; set; }
        public DateTime StartDate{ get; set; }
        public DateTime EndDate { get; set; }
        public string OrganizationsName { get; set; }
        public string ConversationType { get; set; }  
        public int TotalMessageSentCount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal CGSTTaxs { get; set; }
        public decimal SGSTTaxs { get; set; }

        public decimal IGSTTaxs { get; set; }
    }


    public class UsageListFilter
    {
        public string OrganizationsNameFilter { get; set; }
        public string WabaPhoneNumberFilter { get; set; }
        public string WhatsAppDisplayNameFilter { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int PageNumber { get; set; }=1;
        public int PageSize { get; set; }=10;


    }
    public class TemplateMessageInsightsFilter
    {
        public string MetaConfigid { get; set; }
        public string PhoneNumberId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        


    }
}
