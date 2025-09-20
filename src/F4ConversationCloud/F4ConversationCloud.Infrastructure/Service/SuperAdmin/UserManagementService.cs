using F4ConversationCloud.Application.Common.Interfaces.Repositories.SuperAdmin;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Domain.Entities;
using F4ConversationCloud.Domain.Entities.SuperAdmin;
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

        public async Task<int> CreateUpdateAsync(User user)
        {
            int id = await _userManagementRepository.CreateUpdateAsync(new User()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                MobileNo = user.MobileNo,
                Password = user.Password,
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

        public async Task<User> GetUserById(int id)
        {
            User user = await _userManagementRepository.GetByIdAsync(id);

            if (user is null) return null;

            return new User
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                MobileNo = user.MobileNo,
                Password = user.Password,
                Role = user.Role,
                IPAddress = user.IPAddress,
                Designation = user.Designation
            };
        }

        public async Task<List<Role>> GetRolesAsync()
        {
            return await _userManagementRepository.GetRolesAsync();
        }

        public async Task<bool> Activate(int id)
        {
            return await _userManagementRepository.Activate(id);
        }

        public async Task<bool> Deactivate(int id)
        {
            return await _userManagementRepository.Deactivate(id);
        }
    }
}
