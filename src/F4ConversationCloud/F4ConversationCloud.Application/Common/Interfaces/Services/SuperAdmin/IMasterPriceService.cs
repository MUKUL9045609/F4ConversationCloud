using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Domain.Entities.SuperAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin
{
    public interface IMasterPriceService
    {
        Task<int> CreateAsync(MasterPrice masterPrice);
        Task<Tuple<IEnumerable<MasterPrice>, int>> GetFilteredMasterPrices(MasterListFilter filter);
        Task<List<MasterPrice>> GetLatestRecordsByConversationType();
    }
}
