using F4ConversationCloud.Application.Common.Interfaces.Repositories.SuperAdmin;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Infrastructure.Repositories.SuperAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Infrastructure.Service.SuperAdmin
{
    public class ClientUserManagementService : IClientUserManagementService
    {
        private readonly IClientUserManagementRepository _clientUserManagementRepository;

        public ClientUserManagementService(IClientUserManagementRepository clientUserManagementRepository)
        {
            _clientUserManagementRepository = clientUserManagementRepository;
        }

        public async Task<Tuple<IEnumerable<ClientUserListItemModel>, int>> GetFilteredUsers(ClientUserManagementListFilter filter)
        {
            return Tuple.Create(await _clientUserManagementRepository.GetFilteredAsync(filter), await _clientUserManagementRepository.GetCountAsync(filter));
        }

        public async Task<bool> Activate(int id)
        {
            return await _clientUserManagementRepository.Activate(id);
        }

        public async Task<bool> Deactivate(int id)
        {
            return await _clientUserManagementRepository.Deactivate(id);
        }
    }
}
