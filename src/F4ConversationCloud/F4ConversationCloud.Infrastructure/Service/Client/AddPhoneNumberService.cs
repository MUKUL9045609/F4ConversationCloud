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

        public AddPhoneNumberService(IAddPhoneNumberRepository repository, ILogService logService, IMetaRepositories metaRepositories , IMetaService metaService)
        {
            _repository = repository;
            _logService = logService;
            _metaRepositories = metaRepositories;
            _metaService = metaService;
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

        public async Task SyncWhatsAppAccountsAsync(string BusinessId)
        {
            try
            {

                var response = await _metaService.GetBusinessUsersWithWhatsappAccounts();
                //var json = await response.Content.ReadAsStringAsync();

                if (response.Success)
                {
                    var _metaData = response.Data;
                    Root result = JsonConvert.DeserializeObject<Root>(_metaData?.ToString()?? throw new ArgumentNullException("JSON is null"));

                    //var metaDataresponse = JsonConvert.DeserializeObject<dynamic>(_metaData);
                    var businessUsers = _metaData.business_users.data;
                    var list = _metaData.business_users.data;

                    foreach (var user in list)
                    {
                        string businessId = user.id;

                        foreach (var waba in user.assigned_whatsapp_business_accounts.data)
                        {
                            string wabaId = waba.id;

                            if (waba.phone_numbers?.data == null) continue;

                            foreach (var phone in waba.phone_numbers.data)
                            {
                                var record = new WhatsAppAccountTableModel
                                {
                                    BusinessId = businessId,
                                    WABAId = wabaId,
                                    WhatsAppDisplayName = phone.verified_name,
                                    PhoneNumberId = phone.id,
                                    PhoneNumber = phone.display_phone_number,
                                    Status = phone.name_status
                                };

                                // await _metaRepositories.UpdateMetaUsersConfigurationDetails(record);
                            }
                        }
                    }



                    // Now access it safely
                    //var businessUsers = _metaData.business_users.data;
                    foreach (var user in businessUsers)
                    {
                        Console.WriteLine(user.name);
                        foreach (var wa in user.assigned_whatsapp_business_accounts.data)
                        {
                            Console.WriteLine($"- {wa.name} ({wa.id})");
                        }
                    }
                }

                var json = await response.Content.ReadAsStringAsync();
                var metaData = JsonConvert.DeserializeObject<MetaResponse>(json);

                
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
