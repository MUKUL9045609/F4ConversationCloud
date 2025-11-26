using Dapper;
using F4ConversationCloud.Application.Common.Interfaces.Repositories.SuperAdmin;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Infrastructure.Interfaces;
using F4ConversationCloud.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Infrastructure.Repositories.SuperAdmin
{
    public class UsageAndBillingRepository: IUsageAndBillingRepository
    {
        private readonly IGenericRepository<UsageModel> _repository;
        private readonly DbContext _dbContext;


        public UsageAndBillingRepository( IGenericRepository<UsageModel> repository, DbContext dbContext)
        {
               _dbContext = dbContext;
                _repository = repository;
        }

        public async Task<(IEnumerable<UsageModel>,int)> GetUsageDetailsAsync(UsageListFilter filter)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("organizationsNameFilter", filter.OrganizationsNameFilter);
                parameters.Add("wabaPhoneNumberFilter", filter.WabaPhoneNumberFilter);
                parameters.Add("WhatsAppDisplayName", filter.WhatsAppDisplayNameFilter);
                parameters.Add("PageNumber", filter.PageNumber);
                parameters.Add("PageSize", filter.PageSize);

                var list = await _repository.GetListByValuesAsync("sp_GetUsageClientList", parameters);
                int count = await _repository.GetCountAsync("sp_GetUsageClientCount", parameters);

                return (list, count);
            }
            catch (Exception)
            {

               return (Enumerable.Empty<UsageModel>(),0);
            }
           

        }

        public async Task<IEnumerable<TemplateMessageInsightsListViewItem>>GetTemplateMessageInsightsListAsync(TemplateMessageInsightsFilter filter)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@phoneNumberId", filter.PhoneNumberId);
                parameters.Add("@startDate", filter.StartDate);
                parameters.Add("@endDate", filter.EndDate);

                return await _repository.GetListByValuesAsync<TemplateMessageInsightsListViewItem>( "sp_GetTemplateMessageInsightsList", parameters);
            }
            catch (Exception)
            {
                return Enumerable.Empty<TemplateMessageInsightsListViewItem>();
            }
        }
        public async Task<(IEnumerable<BillingListItem>, int)> GetBillingListAsync(BillingListFilter filter)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("organizationsNameFilter", filter.OrganizationsNameFilter);
                parameters.Add("wabaPhoneNumberFilter", filter.WabaPhoneNumberFilter);
                parameters.Add("WhatsAppDisplayName", filter.WhatsAppDisplayNameFilter);
                parameters.Add("PageNumber", filter.PageNumber);
                parameters.Add("PageSize", filter.PageSize);

                var list = await _repository.GetListByValuesAsync<BillingListItem>("sp_GetUsageClientList", parameters);
                int count = await _repository.GetCountAsync("sp_GetUsageClientCount", parameters);

                return (list, count);
            }
            catch (Exception)
            {

                return (Enumerable.Empty<BillingListItem>(), 0);
            }


        }

    }
}

