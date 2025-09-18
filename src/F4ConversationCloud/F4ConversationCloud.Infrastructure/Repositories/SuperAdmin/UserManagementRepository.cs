using Dapper;
using F4ConversationCloud.Application.Common.Interfaces.Repositories.SuperAdmin;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Infrastructure.Interfaces;

namespace F4ConversationCloud.Infrastructure.Repositories.SuperAdmin
{
    public class UserManagementRepository : IUserManagementRepository
    {
        private readonly IGenericRepository<UserManagement> _repository;
        public UserManagementRepository(IGenericRepository<UserManagement> repository)
        {
            _repository = repository;
        }

        public async Task<int> CreateUpdateAsync(UserManagement user)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("id", user.Id);
            parameters.Add("firstName", user.FirstName);
            parameters.Add("lastName", user.LastName);
            parameters.Add("email", user.Email);
            parameters.Add("mobileNo", user.MobileNo);
            parameters.Add("password", user.Password);
            parameters.Add("ipAddress", user.IPAddress);
            parameters.Add("role", user.Role);
            parameters.Add("designation", user.Designation);

            return await _repository.InsertUpdateAsync("sp_CreateUpdateUser", parameters);
        }

        public async Task<int> GetCountAsync(MasterListFilter filter)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("searchString", filter.SearchString);
            parameters.Add("status", filter.Status);

            return await _repository.GetCountAsync("sp_GetUserCount", parameters);
        }

        public async Task<IEnumerable<UserListItemModel>> GetFilteredAsync(MasterListFilter filter)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("searchString", filter.SearchString);
            parameters.Add("status", filter.Status);
            parameters.Add("pageNumber", filter.PageNumber);
            parameters.Add("pageSize", filter.PageSize);

            return await _repository.GetListByValuesAsync<UserListItemModel>("sp_GetFilteredUsers", parameters);
        }
    }
}
