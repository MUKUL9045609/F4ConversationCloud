using Dapper;
using F4ConversationCloud.Application.Common.Interfaces.Repositories.SuperAdmin;
using F4ConversationCloud.Domain.Entities;
using F4ConversationCloud.Domain.Entities.SuperAdmin;
using F4ConversationCloud.Infrastructure.Interfaces;

namespace F4ConversationCloud.Infrastructure.Repositories.SuperAdmin
{
    public class SuperAdminAuthRepository : ISuperAdminAuthRepository
    {
        private readonly IGenericRepository<Auth> _repository;
        public SuperAdminAuthRepository(IGenericRepository<Auth> repository)
        {
            _repository = repository;
        }

        public async Task<Auth> CheckUserExists(string email)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("email", email);

            return await _repository.GetByValuesAsync("sp_CheckUserExists", parameters);
        }

        public async Task<int> UpdatePasswordAsync(ConfirmPasswordModel model)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("id", model.UserId);
            parameters.Add("password", model.Password);

            return await _repository.UpdateValuesAsync("sp_UpdateUserPassword", parameters);
        }
    }
}
