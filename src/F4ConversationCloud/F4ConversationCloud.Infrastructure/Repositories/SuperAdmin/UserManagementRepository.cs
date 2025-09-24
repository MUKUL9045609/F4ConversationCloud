using Dapper;
using F4ConversationCloud.Application.Common.Interfaces.Repositories.SuperAdmin;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Domain.Entities.SuperAdmin;
using F4ConversationCloud.Domain.Extension;
using F4ConversationCloud.Infrastructure.Interfaces;
using F4ConversationCloud.Infrastructure.Persistence;

namespace F4ConversationCloud.Infrastructure.Repositories.SuperAdmin
{
    public class UserManagementRepository : IUserManagementRepository
    {
        private readonly IGenericRepository<User> _repository;
        private readonly DbContext _context;

        public UserManagementRepository(IGenericRepository<User> repository, DbContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<int> CreateUpdateAsync(User user)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("userId", _context.SessionUserId);
            parameters.Add("id", user.Id);
            parameters.Add("firstName", user.FirstName);
            parameters.Add("lastName", user.LastName);
            parameters.Add("email", user.Email);
            parameters.Add("mobileNo", user.MobileNo);
            parameters.Add("password", user.Password.Encrypt());
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

        public async Task<User> GetByIdAsync(int id)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("id", id);

            return await _repository.GetByIdAsync("sp_GetUserById", parameters);
        }

        public async Task<List<Role>> GetRolesAsync()
        {
            DynamicParameters parameters = new DynamicParameters();

            return (await _repository.GetListByParamAsync<Role>("sp_GetRoleDropdown", parameters)).ToList();
        }

        public async Task<bool> Activate(int id)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("Id", id);

            return await _repository.RestoreAsync("sp_ActivateSuperAdminUser", parameters);
        }

        public async Task<bool> Deactivate(int id)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("Id", id);

            return await _repository.DeleteAsync("sp_DeactivateSuperAdminUser", parameters);
        }
    }
}
