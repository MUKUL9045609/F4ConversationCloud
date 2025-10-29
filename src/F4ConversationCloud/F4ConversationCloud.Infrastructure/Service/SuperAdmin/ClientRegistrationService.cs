using F4ConversationCloud.Application.Common.Interfaces.Repositories.SuperAdmin;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Domain.Entities.SuperAdmin;

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

        public async Task<ClientRegistration> GetByIdAsync(int id)
        {
            ClientRegistration cr = await _clientRegistrationRepository.GetByIdAsync(id);

            if (cr is null) return null;

            return new ClientRegistration
            {
                Id = cr.Id,
                FirstName = cr.FirstName,
                LastName = cr.LastName,
                Email = cr.Email,
                ContactNumber = cr.ContactNumber,
                Role = cr.Role,
                RegistrationStatus = cr.RegistrationStatus,
            };
        }
    }
}
