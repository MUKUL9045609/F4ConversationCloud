using F4ConversationCloud.Application.Common.Models.OnBoardingRequestResposeModel;
using F4ConversationCloud.Domain.Entities;


namespace F4ConversationCloud.Application.Common.Interfaces.Services.Onboarding
{
    public interface IOnboardingService
    {
        Task<RegisterUserResponse> RegisterUserAsync(RegisterUserModel request);
       
        Task<ValidateRegistrationOTPResponse> VerifyOTPAsync(ValidateRegistrationOTPModel request);
        
        Task<VarifyUserDetailsResponse> CheckIsMailExitsAsync(VarifyMobileNumberModel request);
        Task<MetaUsersConfigurationResponse> InsertMetaUsersConfigurationAsync(MetaUsersConfiguration request);

        Task<UserDetailsViewModel> GetCustomerByIdAsync(int UserId);
        Task<bool> SendOnboardingConfirmationEmail(VarifyMobileNumberModel request);

        Task<LoginResponse> OnboardingLogin(Loginrequest request);
        Task SendRegistrationSuccessEmailAsync(RegisterUserModel Request);

        Task <bool> ValidateClientEmailAsync(string EmailId);
        Task SendResetPasswordLink(string EmailId);
        Task<bool> SetNewPassword(ConfirmPasswordModel model);
    }
}
