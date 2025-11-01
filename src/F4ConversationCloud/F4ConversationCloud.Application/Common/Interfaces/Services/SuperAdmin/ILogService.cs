using F4ConversationCloud.Application.Common.Models.OnBoardingModel;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin
{
    public interface ILogService
    {
        Task<int> InsertLogAsync(LogModel log);
        Task<int> InsertOnboardingLogs(OnBoardingLogsModel onBoardingLogs);
    }
}
