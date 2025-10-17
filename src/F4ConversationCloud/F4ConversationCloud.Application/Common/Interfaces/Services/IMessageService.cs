
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Domain.Entities;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.OnBoardingRequestResposeModel;
using System.Threading.Tasks;


namespace F4ConversationCloud.Application.Common.Interfaces.Services
{
    public interface IMessageService
    {
       

        //Task SendMessage(string mobileNo, string Text);
        Task<SendSmsResponse> SendVerificationSmsAsync(string mobileNo, string Text);
        Task<bool> SendEmail(EmailRequest Request);
    }
}
