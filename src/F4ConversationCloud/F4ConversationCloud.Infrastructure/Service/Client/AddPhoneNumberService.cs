using F4ConversationCloud.Application.Common.Interfaces.Repositories;
using F4ConversationCloud.Application.Common.Interfaces.Repositories.Client;
using F4ConversationCloud.Application.Common.Interfaces.Services.Client;
using F4ConversationCloud.Application.Common.Interfaces.Services.Common;
using F4ConversationCloud.Application.Common.Interfaces.Services.Meta;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.ClientModel;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using Newtonsoft.Json;

namespace F4ConversationCloud.Infrastructure.Service.Client
{
    public class AddPhoneNumberService : IAddPhoneNumberService
    {
        private readonly IAddPhoneNumberRepository _repository;
        private readonly ILogService _logService;
        private readonly IMetaService _metaService;
        private readonly IMetaRepositories _metaRepositories;

        public AddPhoneNumberService(IAddPhoneNumberRepository repository, ILogService logService, IMetaRepositories metaRepositories)
        {
            _repository = repository;
            _logService = logService;
            _metaRepositories = metaRepositories;
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

        public async Task SyncWhatsAppAccountsAsync()
        {
            try
            {

                var response = await _metaService.GetBusinessUsersWithWhatsappAccounts();
                var json = await response.Content.ReadAsStringAsync();

                var metaData = JsonConvert.DeserializeObject<MetaResponse>(json);

                if (metaData?.BusinessUsers == null)
                    return;

                foreach (var business in metaData.BusinessUsers)
                {
                    string businessId = business.Id;

                    foreach (var waba in business.AssignedWhatsAppBusinessAccounts)
                    {
                        string wabaId = waba.Id;

                        foreach (var phone in waba.PhoneNumbers)
                        {
                            var record = new WhatsAppAccountTableModel
                            {
                                BusinessId = businessId,
                                WABAId = wabaId,
                                WhatsAppDisplayName = phone.VerifiedName,
                                PhoneNumberId = phone.Id,
                                PhoneNumber = phone.DisplayPhoneNumber,
                                Status = phone.NameStatus,
                                BusinessCategory = phone.WhatsappBusinessProfile?.BusinessCategory
                            };

                            await _metaRepositories.UpdateMetaUsersConfigurationDetails(record);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var logModel = new LogModel();
                logModel.Source = "AddPhoneNumber/GetFilteredWhatsAppProfilesByUserId";
                logModel.AdditionalInfo = $"Model: {null}";
                logModel.LogType = "Error";
                logModel.Message = ex.Message;
                logModel.StackTrace = ex.StackTrace;
                await _logService.InsertLogAsync(logModel);
            }
            finally
            {

            }
            //return response;
        }
    }
}
