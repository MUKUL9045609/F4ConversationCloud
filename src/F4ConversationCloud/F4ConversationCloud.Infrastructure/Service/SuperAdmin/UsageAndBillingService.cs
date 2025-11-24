using F4ConversationCloud.Application.Common.Interfaces.Repositories.SuperAdmin;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Infrastructure.Repositories.SuperAdmin;
using Polly.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Infrastructure.Service.SuperAdmin
{
    public class UsageAndBillingService: IUsageAndBillingService
    {
       private readonly IUsageAndBillingRepository _usageAndBillingRepository;
        public UsageAndBillingService(IUsageAndBillingRepository usageAndBillingRepository)
        {
            _usageAndBillingRepository = usageAndBillingRepository;
        }
        public async Task<UsageModelResponse> GetUsageDetailsAsync(UsageListFilter filter)
        {
            var (list, count) = await _usageAndBillingRepository.GetUsageDetailsAsync(filter);

            var result = new UsageModelResponse
            {
                TotalCount = count,
                usageModelsItems = list.Select(item => new UsageModel
                {
                    OrganizationName = item.OrganizationName,
                    ClientId = item.ClientId,

                    items = (item.items ?? new List<UsageMetaConfigurationsListItemModel>())
                        .Select(t => new UsageMetaConfigurationsListItemModel
                        {
                            MetaConfigIds = string.Join(",",
                                (t.MetaConfigIds ?? "")
                                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                .Select(x => x.Trim())
                            ),

                            WhatsAppDisplayName = string.Join(",",
                                (t.WhatsAppDisplayName ?? "")
                                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                .Select(x => x.Trim())
                            ),

                            WabaPhoneNumber = string.Join(",",
                                (t.WabaPhoneNumber ?? "")
                                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                .Select(x => x.Trim())
                            ),
                        })
                        .ToList()

                }).ToList(),
            };

            return result;
        }

    }
}

