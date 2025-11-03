using F4ConversationCloud.Application.Common.Interfaces.Repositories.Common;
using F4ConversationCloud.Application.Common.Interfaces.Services.Common;
using F4ConversationCloud.Application.Common.Models.OnBoardingModel;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using Microsoft.AspNetCore.Http;

namespace F4ConversationCloud.Infrastructure.Service.Common
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LogService(ILogRepository logRepository, IHttpContextAccessor httpContext)
        {
            _logRepository = logRepository;
            _httpContextAccessor = httpContext;
        }

        public async Task<int> InsertLogAsync(LogModel log)
        {
            return await _logRepository.InsertLogAsync(log);
        }
        public async Task<int> InsertOnboardingLogs(OnBoardingLogsModel onBoardingLogs) 
        {
            try
            {
                onBoardingLogs.SessionUserId = _httpContextAccessor.HttpContext.Session.GetInt32("UserId");
                return await _logRepository.InsertOnboardingLogs(onBoardingLogs);
            }
            catch(Exception ex)
            {
                return   0;
            }
          
        }
    }
}
