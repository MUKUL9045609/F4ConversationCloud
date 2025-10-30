using F4ConversationCloud.Application.Common.Interfaces.Repositories.SuperAdmin;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;

namespace F4ConversationCloud.Infrastructure.Service.SuperAdmin
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepository;
        public LogService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task<int> InsertLogAsync(LogModel log)
        {
            return await _logRepository.InsertLogAsync(log);
        }
    }
}
