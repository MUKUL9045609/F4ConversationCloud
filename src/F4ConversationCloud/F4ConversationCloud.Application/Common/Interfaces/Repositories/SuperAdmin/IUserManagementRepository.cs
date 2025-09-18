using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;

namespace F4ConversationCloud.Application.Common.Interfaces.Repositories.SuperAdmin
{
    public interface IUserManagementRepository
    {
        Task<int> CreateUpdateAsync(UserManagement user);
        Task<IEnumerable<UserListItemModel>> GetFilteredAsync(MasterListFilter filter);
        Task<int> GetCountAsync(MasterListFilter filter);
    }
}
