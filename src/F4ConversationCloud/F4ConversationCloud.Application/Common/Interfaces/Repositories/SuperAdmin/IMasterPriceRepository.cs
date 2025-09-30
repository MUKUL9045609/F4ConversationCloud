using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Domain.Entities.SuperAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Interfaces.Repositories.SuperAdmin
{
    public interface IMasterPriceRepository
    {
        Task<int> CreateAsync(MasterPrice masterPrice);
        Task<IEnumerable<MasterPrice>> GetFilteredAsync(MasterListFilter filter);
        Task<int> GetCountAsync(MasterListFilter filter);
    }
}
