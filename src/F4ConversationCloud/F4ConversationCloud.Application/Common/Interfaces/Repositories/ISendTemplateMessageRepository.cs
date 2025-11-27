using F4ConversationCloud.Application.Common.Models;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Interfaces.Repositories
{
    public interface ISendTemplateMessageRepository
    {
        Task<int> InsertIntoTemplateInsights(string PhoneNumberId, int TemplateId, int ConversationType, string MessageSentFrom, string MessageSentTo, string MessageSentStatus);
    }
}
