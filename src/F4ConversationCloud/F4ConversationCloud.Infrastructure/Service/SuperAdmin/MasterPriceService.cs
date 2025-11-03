using F4ConversationCloud.Application.Common.Interfaces.Repositories.SuperAdmin;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Domain.Entities.SuperAdmin;
using Newtonsoft.Json;

namespace F4ConversationCloud.Infrastructure.Service.SuperAdmin
{
    public class MasterPriceService : IMasterPriceService
    {
        private readonly IMasterPriceRepository _masterPriceRepository;
        private readonly ILogService _logService;
        public MasterPriceService(IMasterPriceRepository masterPriceRepository, ILogService logService)
        {
            _masterPriceRepository = masterPriceRepository;
            _logService = logService;
        }

        public async Task<int> CreateAsync(MasterPrice masterPrice)
        {
            var logModel = new LogModel();
            logModel.Source = "MasterPrice/CreateAsync";
            logModel.AdditionalInfo = $"Model: {JsonConvert.SerializeObject(masterPrice)}";
            int response = 0;
            try
            {
                response = await _masterPriceRepository.CreateAsync(new MasterPrice()
                {
                    Id = masterPrice.Id,
                    ConversationType = masterPrice.ConversationType,
                    Price = masterPrice.Price,
                    FromDate = masterPrice.FromDate,
                    ToDate = masterPrice.ToDate
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

        public async Task<Tuple<IEnumerable<MasterPrice>, int>> GetFilteredMasterPrices(MasterListFilter filter)
        {
            Tuple<IEnumerable<MasterPrice>, int> response = Tuple.Create(Enumerable.Empty<MasterPrice>(), 0);
            try
            {
                response = Tuple.Create(await _masterPriceRepository.GetFilteredAsync(filter), await _masterPriceRepository.GetCountAsync(filter));
            }
            catch (Exception ex)
            {
                var logModel = new LogModel();
                logModel.Source = "MasterPrice/GetFilteredMasterPrices";
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

        public async Task<List<MasterPrice>> GetLatestRecordsByConversationType()
        {
            return await _masterPriceRepository.GetLatestRecordsByConversationType();
        }
    }
}
