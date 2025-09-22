using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin
{
    public interface IClientManagementService
    {
        Task<Tuple<IEnumerable<ClientManagementListItemModel>, int>> GetFilteredUsers(MasterListFilter filter);
    }
}
