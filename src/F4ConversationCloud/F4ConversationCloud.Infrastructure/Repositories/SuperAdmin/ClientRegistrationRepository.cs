using Dapper;
using F4ConversationCloud.Application.Common.Interfaces.Repositories.SuperAdmin;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Domain.Entities.SuperAdmin;
using F4ConversationCloud.Infrastructure.Interfaces;
using F4ConversationCloud.Infrastructure.Persistence;

namespace F4ConversationCloud.Infrastructure.Repositories.SuperAdmin
{
    public class ClientRegistrationRepository : IClientRegistrationRepository
    {
        private readonly IGenericRepository<ClientRegistration> _repository;
        private readonly IGenericRepository<RegisteredBusinessDetail> _registeredBusinessDetailRepository;
        private readonly DbContext _context;
        public ClientRegistrationRepository(IGenericRepository<ClientRegistration> repository, DbContext context, IGenericRepository<RegisteredBusinessDetail> registeredBusinessDetailRepository)
        {
            _repository = repository;
            _context = context;
            _registeredBusinessDetailRepository = registeredBusinessDetailRepository;
        }

        public async Task<int> CreateUpdateAsync(ClientRegistration clientRegistration)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("id", clientRegistration.Id);
            parameters.Add("userId", _context.SessionUserId);
            parameters.Add("firstName", clientRegistration.FirstName);
            parameters.Add("lastName", clientRegistration.LastName);
            parameters.Add("email", clientRegistration.Email);
            parameters.Add("contactNumber", clientRegistration.ContactNumber);
            parameters.Add("role", clientRegistration.Role);
            parameters.Add("registrationStatus", clientRegistration.RegistrationStatus);

            return await _repository.InsertUpdateAsync("sp_CreateUpdateClientRegistration", parameters);
        }

        public async Task<int> GetCountAsync(ClientRegistrationListFilter filter)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("nameFilter", filter.NameFilter);
            parameters.Add("emailFilter", filter.EmailFilter);
            parameters.Add("contactNumberFilter", filter.ContactNumberFilter);
            parameters.Add("roleFilter", filter.RoleFilter);
            parameters.Add("createdOnFilter", filter.CreatedOnFilter);
            parameters.Add("updatedOnFilter", filter.UpdatedOnFilter);
            parameters.Add("registrationStatusFilter", filter.RegistrationStatusFilter);

            return await _repository.GetCountAsync("sp_GetClientRegistrationCount", parameters);
        }

        public async Task<IEnumerable<ClientRegistrationListItemModel>> GetFilteredAsync(ClientRegistrationListFilter filter)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("nameFilter", filter.NameFilter);
            parameters.Add("emailFilter", filter.EmailFilter);
            parameters.Add("contactNumberFilter", filter.ContactNumberFilter);
            parameters.Add("roleFilter", filter.RoleFilter);
            parameters.Add("createdOnFilter", filter.CreatedOnFilter);
            parameters.Add("updatedOnFilter", filter.UpdatedOnFilter);
            parameters.Add("registrationStatusFilter", filter.RegistrationStatusFilter);
            parameters.Add("pageNumber", filter.PageNumber);
            parameters.Add("pageSize", filter.PageSize);

            return await _repository.GetListByValuesAsync<ClientRegistrationListItemModel>("sp_GetFilteredClientRegistrations", parameters);
        }

        public async Task<ClientRegistration> GetByIdAsync(int id)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("id", id);

            return await _repository.GetByIdAsync("sp_GetClientRegistrationById", parameters);
        }

        public async Task<bool> CheckEmailExist(string email)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("email", email);

            return await _repository.CheckExistAsync("sp_CheckRegisterEmailExist", parameters);
        }

        public async Task<RegisteredBusinessDetail> GetRegisteredBusinessDetail(int id)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("id", id);

            return await _registeredBusinessDetailRepository.GetByIdAsync("sp_GetRegisteredBusinessDetails", parameters);
        }

        public async Task<bool> CheckContactNumberExist(string contactNumber)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("contactNumber", contactNumber);

            return await _repository.CheckExistAsync("sp_CheckRegisterContactNumberExist", parameters);
        }
    }
}
