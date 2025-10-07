using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Interfaces.Repositories
{
    public interface IAPILogRepository
    {
        Task<int> SaveApiResponseAsync(bool isSuccess, string errorMessage, string rawResponse, int statusCode);
    }
}
