using F4ConversationCloud.Application.Common.Interfaces.Repositories.SuperAdmin;
using F4ConversationCloud.Application.Common.Interfaces.Services.Common;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Infrastructure.Repositories.SuperAdmin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Infrastructure.Service.SuperAdmin
{
    public class ClientUserManagementService : IClientUserManagementService
    {
        private readonly IClientUserManagementRepository _clientUserManagementRepository;
        private readonly ILogService _logService;
        public ClientUserManagementService(IClientUserManagementRepository clientUserManagementRepository, ILogService logService)
        {
            _clientUserManagementRepository = clientUserManagementRepository;
            _logService = logService;
        }

        public async Task<Tuple<IEnumerable<ClientUserListItemModel>, int>> GetFilteredUsers(ClientUserManagementListFilter filter)
        {
            Tuple<IEnumerable<ClientUserListItemModel>, int> response = Tuple.Create(Enumerable.Empty<ClientUserListItemModel>(), 0);
            try
            {
                response = Tuple.Create(await _clientUserManagementRepository.GetFilteredAsync(filter), await _clientUserManagementRepository.GetCountAsync(filter));
            }
            catch (Exception ex)
            {
                var logModel = new LogModel();
                logModel.Source = "ClientUserManagement/GetFilteredUsers";
                logModel.AdditionalInfo = $"Model: {JsonConvert.SerializeObject(filter)}";
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
                response = await _clientUserManagementRepository.Activate(id);
            }
            catch (Exception ex)
            {
                response = true;
                var logModel = new LogModel();
                logModel.Source = "ClientUserManagement/Activate";
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
                response = await _clientUserManagementRepository.Deactivate(id);
            }
            catch (Exception ex)
            {
                response = true;
                var logModel = new LogModel();
                logModel.Source = "ClientUserManagement/Deactivate";
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
