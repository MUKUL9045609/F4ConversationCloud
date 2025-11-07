using F4ConversationCloud.Application.Common.Models;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Interfaces.Services
{
    public interface IEmailSenderService
    {
        Task<bool> Send(EmailRequest message);
    }
}
