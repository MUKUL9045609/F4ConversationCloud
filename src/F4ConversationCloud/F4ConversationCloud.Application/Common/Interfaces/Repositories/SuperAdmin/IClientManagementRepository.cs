using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Interfaces.Repositories.SuperAdmin
{
    public interface IClientManagementRepository
    {
        Task<IEnumerable<ClientManagementListItemModel>> GetFilteredAsync(MasterListFilter filter);
        Task<int> GetCountAsync(MasterListFilter filter);
        Task<ClientDetails> GetClientDetailsById(int Id);
        Task<int> SaveClientPermission(ClientDetails clientDetails);
    }
}
