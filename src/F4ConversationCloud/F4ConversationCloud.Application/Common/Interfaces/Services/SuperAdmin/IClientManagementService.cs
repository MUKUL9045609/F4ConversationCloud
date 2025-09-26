using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.SuperAdmin.Models;

namespace F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin
{
    public interface IClientManagementService
    {
        Task<Tuple<IEnumerable<ClientManagementListItemModel>, int>> GetFilteredUsers(ClientManagementListFilter filter);
        Task<ClientDetails> GetClientDetailsById(int Id);
        Task<int> SaveClientPermission(ClientDetails clientDetails);
        Task<IEnumerable<ClientDetails>> GetClientDetailsByPhoneNumberId(string PhoneNumberId);
        Task<bool> Reject(int Id, string Status, string RejectComment);
    }
}
