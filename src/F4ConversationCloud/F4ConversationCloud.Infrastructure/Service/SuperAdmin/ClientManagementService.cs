using F4ConversationCloud.Application.Common.Interfaces.Repositories.SuperAdmin;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.SuperAdmin.Models;

namespace F4ConversationCloud.Infrastructure.Service.SuperAdmin
{
    public class ClientManagementService : IClientManagementService
    {
        private readonly IClientManagementRepository _clientManagementRepository;
        private readonly ILogService _logService;
        public ClientManagementService(IClientManagementRepository clientManagementRepository, ILogService logService)
        {
            _clientManagementRepository = clientManagementRepository;
            _logService = logService;
        }

        public async Task<Tuple<IEnumerable<ClientManagementListItemModel>, int>> GetFilteredUsers(ClientManagementListFilter filter)
        {
            Tuple<IEnumerable<ClientManagementListItemModel>, int> response = Tuple.Create(Enumerable.Empty<ClientManagementListItemModel>(), 0);
            try
            {
                response = Tuple.Create(await _clientManagementRepository.GetFilteredAsync(filter), await _clientManagementRepository.GetCountAsync(filter));
            }
            catch (Exception ex)
            {
                var logModel = new LogModel();
                logModel.Source = "ClientManagement/GetFilteredUsers";
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

        public async Task<ClientDetails> GetClientDetailsById(int id)
        {
            var response = new ClientDetails();
            try
            {
                response = await _clientManagementRepository.GetClientDetailsById(id);
            }
            catch (Exception ex)
            {
                var logModel = new LogModel();
                logModel.Source = "ClientManagement/GetClientDetailsById";
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

        public async Task<IEnumerable<ClientDetails>> GetClientDetailsByPhoneNumberId(string PhoneNumberId)
        {
            IEnumerable<ClientDetails> response = new List<ClientDetails>();
            try
            {
                response = await _clientManagementRepository.GetClientDetailsByPhoneNumberId(PhoneNumberId);
            }
            catch (Exception ex)
            {
                var logModel = new LogModel();
                logModel.Source = "ClientManagement/GetClientDetailsByPhoneNumberId";
                logModel.AdditionalInfo = $"PhoneNumberId: {PhoneNumberId}";
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

        public async Task<int> SaveClientPermission(ClientDetails clientDetails)
        {
            var logModel = new LogModel();
            logModel.Source = "ClientManagement/SaveClientPermission";
            logModel.AdditionalInfo = $"Model: {clientDetails}";
            int response = 0;
            try
            {
                response = await _clientManagementRepository.SaveClientPermission(clientDetails);
            }
            catch (Exception ex)
            {
                logModel.LogType = "Error";
                logModel.Message = ex.Message;
                logModel.StackTrace = ex.StackTrace;
            }
            finally
            {
                await _logService.InsertLogAsync(logModel);
            }
            return response;
        }

        public async Task<bool> Reject(int Id, string Status, string RejectComment)
        {
            return await _clientManagementRepository.Reject(Id, Status, RejectComment);
        }
    }
}
