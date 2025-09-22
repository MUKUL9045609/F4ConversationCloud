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
        public ClientManagementRepository(IGenericRepository<ClientUser> repository)
        {
            _repository = repository;
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
    }
}
