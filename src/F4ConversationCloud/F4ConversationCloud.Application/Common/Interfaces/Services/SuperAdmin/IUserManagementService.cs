using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Domain.Entities.SuperAdmin;
using F4ConversationCloud.SuperAdmin.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin
{
    public interface IUserManagementService
    {
        Task<int> CreateUpdateAsync(User user);
        Task<Tuple<IEnumerable<UserListItemModel>, int>> GetFilteredUsers(UserManagementListFilter filter);
        Task<User> GetUserById(int id);
        Task<List<Role>> GetRolesAsync();
        Task<bool> Activate(int id);
        Task<bool> Deactivate(int id);
    }
}
