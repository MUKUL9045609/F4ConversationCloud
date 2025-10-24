using F4ConversationCloud.Application.Common.Interfaces.Repositories.SuperAdmin;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Domain.Entities.SuperAdmin;
using F4ConversationCloud.Infrastructure.Repositories.SuperAdmin;
using F4ConversationCloud.SuperAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Rest.Trunking.V1;

namespace F4ConversationCloud.Infrastructure.Service.SuperAdmin
{
    public class ClientRegistrationService : IClientRegistrationService
    {
        private readonly IClientRegistrationRepository _clientRegistrationRepository;
        public ClientRegistrationService(IClientRegistrationRepository clientRegistrationRepository)
        {
            _clientRegistrationRepository = clientRegistrationRepository;
        }

        public async Task<int> CreateUpdateAsync(ClientRegistration clientRegistration)
        {
            return await _clientRegistrationRepository.CreateUpdateAsync(clientRegistration);
        }

        public async Task<Tuple<IEnumerable<ClientRegistrationListItemModel>, int>> GetFilteredRegistrations(ClientRegistrationListFilter filter)
        {
            return Tuple.Create(await _clientRegistrationRepository.GetFilteredAsync(filter), await _clientRegistrationRepository.GetCountAsync(filter));
        }
    }
}
