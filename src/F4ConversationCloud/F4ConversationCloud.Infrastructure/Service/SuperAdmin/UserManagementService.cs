using F4ConversationCloud.Application.Common.Interfaces.Repositories.SuperAdmin;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Domain.Extension;

namespace F4ConversationCloud.Infrastructure.Service.SuperAdmin
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IUserManagementRepository _userManagementRepository;

        public UserManagementService(IUserManagementRepository userManagementRepository)
        {
            _userManagementRepository = userManagementRepository;
        }

        public async Task<int> CreateUpdateAsync(UserManagement user)
        {
            int id = await _userManagementRepository.CreateUpdateAsync(new UserManagement()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                MobileNo = user.MobileNo,
                Password = user.Password.Encrypt(),
                IPAddress = user.IPAddress,
                Role = user.Role,
                Designation = user.Designation
            });

            return id;
        }

        public async Task<Tuple<IEnumerable<UserListItemModel>, int>> GetFilteredUsers(MasterListFilter filter)
        {
            return Tuple.Create(await _userManagementRepository.GetFilteredAsync(filter), await _userManagementRepository.GetCountAsync(filter));
        }
    }
}
