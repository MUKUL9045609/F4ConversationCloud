using Dapper;
using F4ConversationCloud.Application.Common.Interfaces.Repositories.Common;
using F4ConversationCloud.Application.Common.Models.OnBoardingModel;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Infrastructure.Interfaces;
using F4ConversationCloud.Infrastructure.Persistence;

namespace F4ConversationCloud.Infrastructure.Repositories.Common
{
    public class LogRepository : ILogRepository
    {
        private readonly IGenericRepository<LogModel> _repository;
        private readonly DbContext _context;
        public LogRepository(IGenericRepository<LogModel> repository, DbContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<int> InsertLogAsync(LogModel log)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("userId", _context.SessionUserId);
            parameters.Add("Source", log.Source);
            parameters.Add("LogDate", log.LogDate);
            parameters.Add("LogType", log.LogType);
            parameters.Add("Message", log.Message);
            parameters.Add("StackTrace", log.StackTrace);
            parameters.Add("AdditionalInfo", log.AdditionalInfo);

            return await _repository.InsertUpdateAsync("sp_InsertSuperAdminLog", parameters);
        }

        public async Task<int> InsertOnboardingLogs(OnBoardingLogsModel onBoardingLogs)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("userId", onBoardingLogs.SessionUserId);
            parameters.Add("Source", onBoardingLogs.Source);
            parameters.Add("LogDate", onBoardingLogs.LogDate);
            parameters.Add("LogType", onBoardingLogs.LogType);
            parameters.Add("Message", onBoardingLogs.Message);
            parameters.Add("StackTrace", onBoardingLogs.StackTrace);
            parameters.Add("AdditionalInfo", onBoardingLogs.AdditionalInfo);
            return await _repository.InsertUpdateAsync("sp_InsertOnboardingLog", parameters);
        }
    }
}
