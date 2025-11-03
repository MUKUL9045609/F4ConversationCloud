using F4ConversationCloud.Application.Common.Interfaces.Repositories.SuperAdmin;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Domain.Entities.SuperAdmin;
using F4ConversationCloud.Infrastructure.Repositories.SuperAdmin;
using Newtonsoft.Json;

namespace F4ConversationCloud.Infrastructure.Service.SuperAdmin
{
    public class MasterTaxService : IMasterTaxService
    {
        private readonly IMasterTaxRepository _masterTaxRepository;
        private readonly ILogService _logService;
        public MasterTaxService(IMasterTaxRepository masterTaxRepository, ILogService logService)
        {
            _masterTaxRepository = masterTaxRepository;
            _logService = logService;
        }

        public async Task<int> CreateUpdateAsync(MasterTax masterTax)
        {
            var logModel = new LogModel();
            logModel.Source = "MasterTax/CreateUpdateAsync";
            logModel.AdditionalInfo = $"Model: {JsonConvert.SerializeObject(masterTax)}";
            int response = 0;
            try
            {
                response = await _masterTaxRepository.CreateUpdateAsync(new MasterTax()
                {
                    Id = masterTax.Id,
                    SGST = masterTax.SGST,
                    CGST = masterTax.CGST,
                    IGST = masterTax.IGST
                });
                logModel.LogType = "Success";
                logModel.Message = "Record created successfully";
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

        public async Task<MasterTax> GetMasterTaxAsync()
        {
            var response = new MasterTax();
            try
            {
                response = await _masterTaxRepository.GetMasterTaxAsync();
            }
            catch (Exception ex)
            {
                var logModel = new LogModel();
                logModel.Source = "MasterTax/GetMasterTaxAsync";
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
