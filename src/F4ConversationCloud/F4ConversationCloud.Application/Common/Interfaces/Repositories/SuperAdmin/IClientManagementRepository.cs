using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.SuperAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Interfaces.Repositories.SuperAdmin
{
    public interface IClientManagementRepository
    {
        Task<IEnumerable<ClientManagementListItemModel>> GetFilteredAsync(ClientManagementListFilter filter);
        Task<int> GetCountAsync(ClientManagementListFilter filter);
        Task<ClientDetails> GetClientDetailsById(int Id);
        Task<int> SaveClientPermission(ClientDetails clientDetails);
        Task<IEnumerable<ClientDetails>> GetClientDetailsByPhoneNumberId(string PhoneNumberId);
        Task<bool> Reject(int Id, string Status, string RejectComment);
    }
}
