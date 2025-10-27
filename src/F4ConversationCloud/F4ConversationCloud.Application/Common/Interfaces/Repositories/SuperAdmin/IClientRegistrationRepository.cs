using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Domain.Entities.SuperAdmin;
using F4ConversationCloud.SuperAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Interfaces.Repositories.SuperAdmin
{
    public interface IClientRegistrationRepository
    {
        Task<int> CreateUpdateAsync(ClientRegistration clientRegistration);
        Task<IEnumerable<ClientRegistrationListItemModel>> GetFilteredAsync(ClientRegistrationListFilter filter);
        Task<int> GetCountAsync(ClientRegistrationListFilter filter);
        Task<ClientRegistration> GetByIdAsync(int id);
    }
}
