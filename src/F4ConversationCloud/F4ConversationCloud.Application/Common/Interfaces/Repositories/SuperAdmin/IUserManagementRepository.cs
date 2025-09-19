using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Domain.Entities.SuperAdmin;

namespace F4ConversationCloud.Application.Common.Interfaces.Repositories.SuperAdmin
{
    public interface IUserManagementRepository
    {
        Task<int> CreateUpdateAsync(User user);
        Task<IEnumerable<UserListItemModel>> GetFilteredAsync(MasterListFilter filter);
        Task<int> GetCountAsync(MasterListFilter filter);
        Task<User> GetByIdAsync(int id);
        Task<List<Role>> GetRolesAsync();
        Task<bool> Activate(int id);
        Task<bool> Deactivate(int id);
    }
}
