using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin
{
    public interface IUsageAndBillingService
    {
        Task<UsageModelResponse> GetUsageListAsync(UsageListFilter filter);
        Task<BillingListModelResponse> GetBillingListAsync(BillingListFilter filter);
        Task<IEnumerable<TemplateMessageInsightsListViewItem>> GetTemplateMessageInsightsListAsync(TemplateMessageInsightsFilter filter);
    }
}
