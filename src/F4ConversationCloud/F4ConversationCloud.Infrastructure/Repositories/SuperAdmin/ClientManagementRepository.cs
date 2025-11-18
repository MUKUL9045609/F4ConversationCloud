using Dapper;
using F4ConversationCloud.Application.Common.Interfaces.Repositories.SuperAdmin;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Domain.Entities.SuperAdmin;
using F4ConversationCloud.Infrastructure.Interfaces;
using F4ConversationCloud.Infrastructure.Persistence;
using F4ConversationCloud.SuperAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Infrastructure.Repositories.SuperAdmin
{
    public class ClientManagementRepository: IClientManagementRepository
    {
        private readonly IGenericRepository<ClientUser> _repository;
        private readonly IGenericRepository<ClientDetails> _repositoryDetails;
        private readonly DbContext _context;

        public ClientManagementRepository(IGenericRepository<ClientUser> repository, IGenericRepository<ClientDetails> repositoryDetails, DbContext context)
        {
            _repository = repository;
            _repositoryDetails = repositoryDetails;
            _context = context;
        }

        public async Task<int> GetCountAsync(ClientManagementListFilter filter)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("clientNameSearch", filter.ClientNameSearch);
            parameters.Add("statusFilter", filter.StatusFilter);
            parameters.Add("onboardingOnFilter", filter.OnboardingOnFilter);
            parameters.Add("phoneNumberFilter", filter.PhoneNumberFilter);
            parameters.Add("registrationId", filter.RegistrationId);
            parameters.Add("wabaAccountStatusFilter", filter.WabaAccountStatusFilter);

            return await _repository.GetCountAsync("sp_GetClientsCount_v2", parameters);
        }

        public async Task<IEnumerable<ClientManagementListItemModel>> GetFilteredAsync(ClientManagementListFilter filter)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("clientNameSearch", filter.ClientNameSearch);
            parameters.Add("statusFilter", filter.StatusFilter);
            parameters.Add("onboardingOnFilter", filter.OnboardingOnFilter);
            parameters.Add("phoneNumberFilter", filter.PhoneNumberFilter);
            parameters.Add("pageNumber", filter.PageNumber);
            parameters.Add("pageSize", filter.PageSize);
            parameters.Add("registrationId", filter.RegistrationId);
            parameters.Add("wabaAccountStatusFilter", filter.WabaAccountStatusFilter);  
            return await _repository.GetListByValuesAsync<ClientManagementListItemModel>("sp_GetFilteredClients_v2", parameters);
        }

        public async Task<(IEnumerable<BusinessAccountListItem>,int)> GetBusinessAccountListAsync(BusinessAccountListFilter filter)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();

                parameters.Add("organizationsNameFilter", filter.OrganizationsFilter);
                parameters.Add("createdOnFilter", filter.OnboardingOnFilter);
                parameters.Add("phoneNumberFilter", filter.PhoneNumberFilter);
                parameters.Add("pageNumber", filter.PageNumber);
                parameters.Add("pageSize", filter.PageSize);

                var List = await _repository.GetListByValuesAsync<BusinessAccountListItem>("sp_GetBusinessAccountList", parameters);
                int Count = await _repository.GetCountAsync("sp_GetBusinessAccountListCount", parameters);

                return (List, Count);
            }
            catch (Exception)
            {

               return (Enumerable.Empty<BusinessAccountListItem>(), 0);
            }
            
        }




        public async Task<ClientDetails> GetClientDetailsById(int id)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("id", id);

            return await _repositoryDetails.GetByIdAsync("sp_GetClientDetailsById", parameters);
        }

        public async Task<IEnumerable<ClientDetails>> GetClientDetailsByPhoneNumberId(string PhoneNumberId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("PhoneNumberId", PhoneNumberId);

            return await _repositoryDetails.GetListByValuesAsync<ClientDetails>("sp_GetClientDetailsByPhoneNumberId", parameters);
        }

        public async Task<int> SaveClientPermission(ClientDetails clientDetails)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("MetaUserConfigurationId", clientDetails.Id);
            parameters.Add("userId", _context.SessionUserId);
            parameters.Add("TemplateType", clientDetails.TemplateType);
            parameters.Add("Create", clientDetails.Create);
            parameters.Add("Add", clientDetails.Add);
            parameters.Add("Edit", clientDetails.Edit);
            parameters.Add("Delete", clientDetails.Delete);
            parameters.Add("All", clientDetails.All);
            parameters.Add("AllowUserManagement", clientDetails.AllowUserManagement);

            return await _repository.InsertUpdateAsync("sp_SaveClientPermission", parameters);
        }

        public async Task<bool> Reject(int Id, string status, string comment)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("userId", _context.SessionUserId);
            parameters.Add("Id", Id);
            parameters.Add("status", status);
            parameters.Add("comment", comment);

            return await _repository.InsertMultipleAsync("sp_UpdateClientRejectStatus", parameters);
        }
    }
}
