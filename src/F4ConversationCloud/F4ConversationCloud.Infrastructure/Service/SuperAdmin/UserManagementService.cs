using F4ConversationCloud.Application.Common.Interfaces.Repositories.SuperAdmin;
using F4ConversationCloud.Application.Common.Interfaces.Services.Common;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Domain.Entities.SuperAdmin;
using F4ConversationCloud.Infrastructure.Repositories.SuperAdmin;
using F4ConversationCloud.SuperAdmin.Models;
using Newtonsoft.Json;

namespace F4ConversationCloud.Infrastructure.Service.SuperAdmin
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IUserManagementRepository _userManagementRepository;
        private readonly ILogService _logService;

        public UserManagementService(IUserManagementRepository userManagementRepository, ILogService logService)
        {
            _userManagementRepository = userManagementRepository;
            _logService = logService;
        }

        public async Task<int> CreateUpdateAsync(User user)
        {
            int response = 0;
            try
            {
                response = await _userManagementRepository.CreateUpdateAsync(new User()
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
            }
            catch (Exception ex)
            {
                var logModel = new LogModel();
                logModel.Source = "UserManagement/CreateUpdateAsync";
                logModel.AdditionalInfo = $"User: {JsonConvert.SerializeObject(user)}";
                logModel.LogType = "Error";
                logModel.Message = ex.Message;
                logModel.StackTrace = ex.StackTrace;
                await _logService.InsertLogAsync(logModel);
            }
            finally
            {

            }
            return response;
        }

        public async Task<Tuple<IEnumerable<UserListItemModel>, int>> GetFilteredUsers(UserManagementListFilter filter)
        {
            Tuple<IEnumerable<UserListItemModel>, int> response = Tuple.Create(Enumerable.Empty<UserListItemModel>(), 0);
            try
            {
                response = Tuple.Create(await _userManagementRepository.GetFilteredAsync(filter), await _userManagementRepository.GetCountAsync(filter));
            }
            catch (Exception ex)
            {
                var logModel = new LogModel();
                logModel.Source = "UserManagement/GetFilteredUsers";
                logModel.AdditionalInfo = $"Model: {filter}";
                logModel.LogType = "Error";
                logModel.Message = ex.Message;
                logModel.StackTrace = ex.StackTrace;
                await _logService.InsertLogAsync(logModel);
            }
            finally
            {

            }
            return response;
        }

        public async Task<User> GetUserById(int id)
        {
            var response = new User();
            try
            {
                response = await _userManagementRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                var logModel = new LogModel();
                logModel.Source = "UserManagement/GetUserById";
                logModel.AdditionalInfo = $"Id: {id}";
                logModel.LogType = "Error";
                logModel.Message = ex.Message;
                logModel.StackTrace = ex.StackTrace;
                await _logService.InsertLogAsync(logModel);
            }
            finally
            {

            }
            return response;
        }

        public async Task<List<Role>> GetRolesAsync()
        {
            var response = new List<Role>();
            try
            {
                response = await _userManagementRepository.GetRolesAsync();
            }
            catch (Exception ex)
            {
                var logModel = new LogModel();
                logModel.Source = "UserManagement/GetUserById";
                logModel.LogType = "Error";
                logModel.Message = ex.Message;
                logModel.StackTrace = ex.StackTrace;
                await _logService.InsertLogAsync(logModel);
            }
            finally
            {

            }
            return response;
        }

        public async Task<bool> Activate(int id)
        {
            bool response = false;
            try
            {
                response = await _userManagementRepository.Activate(id);
            }
            catch (Exception ex)
            {
                response = true;
                var logModel = new LogModel();
                logModel.Source = "UserManagement/Activate";
                logModel.AdditionalInfo = $"Id: {id}";
                logModel.LogType = "Error";
                logModel.Message = ex.Message;
                logModel.StackTrace = ex.StackTrace;
                await _logService.InsertLogAsync(logModel);
            }
            finally
            {

            }
            return response;
        }

        public async Task<bool> Deactivate(int id)
        {
            bool response = false;
            try
            {
                response = await _userManagementRepository.Deactivate(id);
            }
            catch (Exception ex)
            {
                response = true;
                var logModel = new LogModel();
                logModel.Source = "UserManagement/Deactivate";
                logModel.AdditionalInfo = $"Id: {id}";
                logModel.LogType = "Error";
                logModel.Message = ex.Message;
                logModel.StackTrace = ex.StackTrace;
                await _logService.InsertLogAsync(logModel);
            }
            finally
            {

            }
            return response;
        }
    }
}
