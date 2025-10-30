using Dapper;
using F4ConversationCloud.Application.Common.Interfaces.Repositories.SuperAdmin;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Domain.Entities.SuperAdmin;
using F4ConversationCloud.Infrastructure.Interfaces;
using F4ConversationCloud.Infrastructure.Persistence;

namespace F4ConversationCloud.Infrastructure.Repositories.SuperAdmin
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
    }
}
