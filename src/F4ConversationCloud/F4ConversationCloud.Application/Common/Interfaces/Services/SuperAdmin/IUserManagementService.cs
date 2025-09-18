using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;

namespace F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin
{
    public interface IUserManagementService
    {
        Task<int> CreateUpdateAsync(UserManagement user);
        Task<Tuple<IEnumerable<UserListItemModel>, int>> GetFilteredUsers(MasterListFilter filter);
    }
}
