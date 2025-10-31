
using F4ConversationCloud.Application.Common.Models.OnBoardingModel;
using F4ConversationCloud.Application.Common.Models.OnBoardingRequestResposeModel;
using System.Threading.Tasks;


namespace F4ConversationCloud.Application.Common.Interfaces.Services
{
    public interface IMessageService
    {

        Task<SendSmsResponse> SendVerificationSmsAsync(string mobileNo, string Text);
        Task<OnboardingContactNoVerificationResponse> SendOnboardingVerificationAsync(VarifyMobileNumberModel request);
    }
}
