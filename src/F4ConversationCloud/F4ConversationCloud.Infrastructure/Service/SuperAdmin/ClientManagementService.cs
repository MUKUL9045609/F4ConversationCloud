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
    public class ClientManagementService: IClientManagementService
    {
        private readonly IClientManagementRepository _clientManagementRepository;
        public ClientManagementService(IClientManagementRepository clientManagementRepository)
        {
            _clientManagementRepository = clientManagementRepository;
        }

        public async Task<Tuple<IEnumerable<ClientManagementListItemModel>, int>> GetFilteredUsers(MasterListFilter filter)
        {
            return Tuple.Create(await _clientManagementRepository.GetFilteredAsync(filter), await _clientManagementRepository.GetCountAsync(filter));
        }

        public async Task<ClientDetails> GetClientDetailsById(int id)
        {
            return await _clientManagementRepository.GetClientDetailsById(id);
        }

        public async Task<int> SaveClientPermission(ClientDetails clientDetails)
        {
            return await _clientManagementRepository.SaveClientPermission(clientDetails);
        }
    }
}
