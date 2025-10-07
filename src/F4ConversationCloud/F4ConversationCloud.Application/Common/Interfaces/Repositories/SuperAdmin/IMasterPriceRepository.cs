using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Domain.Entities.SuperAdmin;

namespace F4ConversationCloud.Application.Common.Interfaces.Repositories.SuperAdmin
{
    public interface IMasterPriceRepository
    {
        Task<int> CreateAsync(MasterPrice masterPrice);
        Task<IEnumerable<MasterPrice>> GetFilteredAsync(MasterListFilter filter);
        Task<int> GetCountAsync(MasterListFilter filter);
        Task<List<MasterPrice>> GetLatestRecordsByConversationType();
    }
}
