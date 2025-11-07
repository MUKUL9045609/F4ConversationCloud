using F4ConversationCloud.Application.Common.Models.ClientModel;
using F4ConversationCloud.Application.Common.Models.OnBoardingModel;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Interfaces.Services.Common
{
    public interface ILogService
    {
        Task<int> InsertLogAsync(LogModel log);
        Task<int> InsertOnboardingLogs(OnBoardingLogsModel onBoardingLogs);
        Task<int> InsertClientAdminLogsAsync(ClientAdminLogsModel clientAdminLogs);
    }
}
