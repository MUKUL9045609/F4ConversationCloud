using F4ConversationCloud.Application.Common.Interfaces.Repositories.Client;
using F4ConversationCloud.Application.Common.Interfaces.Services.Client;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models.ClientModel;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.SuperAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Infrastructure.Service.Client
{
    public class AddPhoneNumberService : IAddPhoneNumberService
    {
        private readonly IAddPhoneNumberRepository _repository;
        private readonly ILogService _logService;
        public AddPhoneNumberService(IAddPhoneNumberRepository repository, ILogService logService)
        {
            _repository = repository;
            _logService = logService;
        }

        public async Task<IEnumerable<AddPhoneNumberModel>> GetWhatsAppProfilesByUserId()
        {
            return await _repository.GetWhatsAppProfilesByUserId();
        }

        public async Task<Tuple<IEnumerable<AddPhoneNumberModel>, int>> GetFilteredWhatsAppProfilesByUserId(AddPhoneNumberListFilter filter)
        {
            Tuple<IEnumerable<AddPhoneNumberModel>, int> response = Tuple.Create(Enumerable.Empty<AddPhoneNumberModel>(), 0);
            try
            {
                response = Tuple.Create(await _repository.GetFilteredAsync(filter), await _repository.GetCountAsync(filter));
            }
            catch (Exception ex)
            {
                var logModel = new LogModel();
                logModel.Source = "AddPhoneNumber/GetFilteredWhatsAppProfilesByUserId";
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
    }
}
