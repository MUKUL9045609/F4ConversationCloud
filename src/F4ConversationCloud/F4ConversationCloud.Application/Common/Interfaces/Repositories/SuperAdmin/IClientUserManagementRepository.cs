using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Interfaces.Repositories.SuperAdmin
{
    public interface IClientUserManagementRepository
    {
        Task<IEnumerable<ClientUserListItemModel>> GetFilteredAsync(ClientUserManagementListFilter filter);
        Task<int> GetCountAsync(ClientUserManagementListFilter filter);
        Task<bool> Activate(int id);
        Task<bool> Deactivate(int id);
    }
}
