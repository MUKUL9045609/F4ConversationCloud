using F4ConversationCloud.Application.Common.Models;

namespace F4ConversationCloud.Application.Common.Interfaces.Services
{
    public interface IAPILogService
    {
        Task GenerateLog(APILogModel aPILogModel);

        Task CreateLog(string fileName, string newContent);
    }
}
