using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Domain.Entities.SuperAdmin;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin
{
    public interface IClientRegistrationService
    {
        Task<int> CreateUpdateAsync(ClientRegistration clientRegistration);
        Task<Tuple<IEnumerable<ClientRegistrationListItemModel>, int>> GetFilteredRegistrations(ClientRegistrationListFilter filter);
        Task<ClientRegistration> GetByIdAsync(int id);
        Task SendRegistrationEmailAsync(string email, string name, int id, string contactNumber);
        Task<bool> CheckEmailExist(string email);
        Task<RegisteredBusinessDetail> GetRegisteredBusinessDetail(int id);
        Task<bool> CheckContactNumberExist(string contactNumber);
        Task<CommonSuperAdminServiceResponse> DeactivateClientAccountAsync(ActivateDeactivateClientAccountRequest request);
        Task<CommonSuperAdminServiceResponse> ActivateClientAccountAsync(ActivateDeactivateClientAccountRequest request);
    }
}
