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
        public string OrganizationName { get; set; }
        public string ClientId { get; set; }
        public string WhatsAppDisplayName { get; set; }
        public string WabaPhoneNumber { get; set; }
        public string MetaConfigIds { get; set; }

        public List<UsageMetaConfigurationsListItemModel> items { get; set; }

    }
    public class UsageModelResponse
    {
        public string OrganizationName { get; set; }
        public string ClientId { get; set; }
        public string WhatsAppDisplayName { get; set; }
        public string WabaPhoneNumber { get; set; }
        public string MetaConfigIds { get; set; }
        public int TotalCount { get; set; }
        public List<UsageModel>  usageModelsItems { get; set; }

    }
    public class UsageMetaConfigurationsListItemModel
    {
        public string MetaConfigIds { get; set; }
        public string WhatsAppDisplayName { get; set; }
        public string WabaPhoneNumber { get; set; }
       
    }
    public class TemplateInsightsListViewItem
    { 
        public string ConversationType { get; set; }
        public string MessageSentCount { get; set; }
        public string MessageSentAmount { get; set; }
    }
    public class UsageListFilter
    {
        public string OrganizationsNameFilter { get; set; }
        public string WabaPhoneNumberFilter { get; set; }
        public string WhatsAppDisplayNameFilter { get; set; }
        public int PageNumber { get; set; }=1;
        public int PageSize { get; set; }=10;


    }
}
