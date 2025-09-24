using Dapper;
using F4ConversationCloud.Application.Common.Interfaces.Repositories.SuperAdmin;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Domain.Entities.SuperAdmin;
using F4ConversationCloud.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Infrastructure.Repositories.SuperAdmin
{
    public class ClientManagementRepository: IClientManagementRepository
    {
        private readonly IGenericRepository<ClientUser> _repository;
        private readonly IGenericRepository<ClientDetails> _repositoryDetails;
        public ClientManagementRepository(IGenericRepository<ClientUser> repository, IGenericRepository<ClientDetails> repositoryDetails)
        {
            _repository = repository;
            _repositoryDetails = repositoryDetails;
        }

        public async Task<int> GetCountAsync(MasterListFilter filter)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("searchString", filter.SearchString);
            parameters.Add("status", filter.Status);

            return await _repository.GetCountAsync("sp_GetClientsCount", parameters);
        }

        public async Task<IEnumerable<ClientManagementListItemModel>> GetFilteredAsync(MasterListFilter filter)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("searchString", filter.SearchString);
            parameters.Add("status", filter.Status);
            parameters.Add("pageNumber", filter.PageNumber);
            parameters.Add("pageSize", filter.PageSize);

            return await _repository.GetListByValuesAsync<ClientManagementListItemModel>("sp_GetFilteredClients", parameters);
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

            parameters.Add("IsMarketing", clientDetails.IsMarketing);
            parameters.Add("IsAuthentication", clientDetails.IsAuthentication);
            parameters.Add("IsUtility", clientDetails.IsUtility);
            parameters.Add("MarketingCreate", clientDetails.MarketingCreate);
            parameters.Add("MarketingAdd", clientDetails.MarketingAdd);
            parameters.Add("MarketingEdit", clientDetails.MarketingEdit);
            parameters.Add("MarketingDelete", clientDetails.MarketingDelete);
            parameters.Add("MarketingAll", clientDetails.MarketingAll);
            parameters.Add("AuthenticationCreate", clientDetails.AuthenticationCreate);
            parameters.Add("AuthenticationAdd", clientDetails.AuthenticationAdd);
            parameters.Add("AuthenticationEdit", clientDetails.AuthenticationEdit);
            parameters.Add("AuthenticationDelete", clientDetails.AuthenticationDelete);
            parameters.Add("AuthenticationAll", clientDetails.AuthenticationAll);
            parameters.Add("UtilityCreate", clientDetails.UtilityCreate);
            parameters.Add("UtilityAdd", clientDetails.UtilityAdd);
            parameters.Add("UtilityEdit", clientDetails.UtilityEdit);
            parameters.Add("UtilityDelete", clientDetails.UtilityDelete);
            parameters.Add("UtilityAll", clientDetails.UtilityAll);
            
            return await _repository.InsertUpdateAsync("sp_SaveClientPermission", parameters);
        }
    }
}
