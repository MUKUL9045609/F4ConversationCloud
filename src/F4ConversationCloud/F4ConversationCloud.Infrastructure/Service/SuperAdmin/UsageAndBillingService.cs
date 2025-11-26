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

            var usageModels = new List<UsageModel>();

            foreach (var item in list)
            {
                var phoneNumbers = (item.phoneNumberId ?? "")
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .ToList();

                var templateResults = new List<TemplateMessageInsightsListViewItem>();
                foreach (var phone in phoneNumbers)
                {
                    var result = await _usageAndBillingRepository.GetTemplateMessageInsightsListAsync(new TemplateMessageInsightsFilter
                    {
                        StartDate = filter.StartDate,
                        EndDate = filter.EndDate,
                        PhoneNumberId = phone
                    });

                    templateResults.AddRange(result);
                }
                var phoneIds = string.Join(",", phoneNumbers);
                usageModels.Add(new UsageModel
                {
                    SrNo = item.SrNo,
                    OrganizationsName = item.OrganizationsName,
                    ClientInfoId = item.ClientInfoId,
                    ClientId = item.ClientId,
                    WabaPhoneNumber = string.Join(",", (item.WabaPhoneNumber ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim())),
                    WhatsAppDisplayName = string.Join(",", (item.WhatsAppDisplayName ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim())),
                    phoneNumberId = phoneIds,
                    TemplateInsightsList = templateResults
                });
            }
            return new UsageModelResponse
            {
                TotalCount = count,
                usageModelsItems = usageModels
            };
        }


    }
}

