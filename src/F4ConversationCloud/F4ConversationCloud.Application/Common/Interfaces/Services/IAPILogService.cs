using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Domain.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Interfaces.Services
{
    public interface IAPILogService
    {
        Task GenerateLog(APILogModel aPILogModel);

        Task CreateLog(string fileName, string newContent);

        Task<T> CallExternalAPI<T>(string EndPoint, string MethodType, T RequestBody, Dictionary<string, string> OAUths, string APIType, Dictionary<string, string> QuerryStringParameters, bool AddLogs = true, bool IsExternalAPI = false, string BodyType = "json");
    }
}
