using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Interfaces.Repositories.SuperAdmin
{
    public interface IUsageAndBillingRepository
    {
        Task<(IEnumerable<UsageModel>, int)> GetUsageDetailsAsync(UsageListFilter filter);
        Task<IEnumerable<TemplateMessageInsightsListViewItem>> GetTemplateMessageInsightsListAsync(TemplateMessageInsightsFilter filter);
        Task<(IEnumerable<BillingListItem>, int)> GetBillingListAsync(BillingListFilter filter);
    }
}
