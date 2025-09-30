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
    public class ClientUserManagementRepository : IClientUserManagementRepository
    {
        private readonly IGenericRepository<ClientUser> _repository;
        public ClientUserManagementRepository(IGenericRepository<ClientUser> repository)
        {
            _repository = repository;
        }

        public async Task<int> GetCountAsync(ClientUserManagementListFilter filter)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("businessFilter", filter.BusinessFilter);
            parameters.Add("nameFilter", filter.NameFilter);
            parameters.Add("emailFilter", filter.EmailFilter);
            parameters.Add("roleFilter", filter.RoleFilter);
            parameters.Add("createdOnFilter", filter.CreatedOnFilter);
            parameters.Add("updatedOnFilter", filter.UpdatedOnFilter);
            parameters.Add("status", filter.Status);

            return await _repository.GetCountAsync("sp_GetClientUserCount", parameters);
        }

        public async Task<IEnumerable<ClientUserListItemModel>> GetFilteredAsync(ClientUserManagementListFilter filter)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("businessFilter", filter.BusinessFilter);
            parameters.Add("nameFilter", filter.NameFilter);
            parameters.Add("emailFilter", filter.EmailFilter);
            parameters.Add("roleFilter", filter.RoleFilter);
            parameters.Add("createdOnFilter", filter.CreatedOnFilter);
            parameters.Add("updatedOnFilter", filter.UpdatedOnFilter);
            parameters.Add("status", filter.Status);
            parameters.Add("pageNumber", filter.PageNumber);
            parameters.Add("pageSize", filter.PageSize);

            return await _repository.GetListByValuesAsync<ClientUserListItemModel>("sp_GetFilteredClientUsers", parameters);
        }
    }
}
