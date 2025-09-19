using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Domain.Entities.SuperAdmin;

namespace F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin
{
    public interface IUserManagementService
    {
        Task<int> CreateUpdateAsync(User user);
        Task<Tuple<IEnumerable<UserListItemModel>, int>> GetFilteredUsers(MasterListFilter filter);
        Task<User> GetUserById(int id);
        Task<List<Role>> GetRolesAsync();
        Task<bool> Activate(int id);
        Task<bool> Deactivate(int id);
    }
}
