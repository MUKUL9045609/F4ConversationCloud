using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using F4ConversationCloud.Application.Common.Models.OnBoardingRequestResposeModel;

namespace F4ConversationCloud.Application.Common.Interfaces.IWebServices
{
    public interface IOnboardingService
    {
        Task<RegisterUserResponse> RegisterUserAsync(RegisterUserModel request);
       
        Task<ValidateRegistrationOTPResponse> VerifyOTPAsync(ValidateRegistrationOTPModel request);
        
        Task<VarifyUserDetailsResponse> CheckMailOrPhoneNumberAsync(VarifyMobileNumberModel request);
        Task<MetaUsersConfigurationResponse> InsertMetaUsersConfigurationAsync(MetaUsersConfiguration request);

        Task<UserDetailsViewModel> GetCustomerByIdAsync(int UserId);
        Task<bool> SendOnboardingConfirmationEmail(VarifyMobileNumberModel request);

        Task<LoginResponse> OnboardingLogin(Loginrequest request);
    }
}
