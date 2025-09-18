using F4ConversationCloud.Application.Common.Models.OnBoardingModel;
using F4ConversationCloud.Application.Common.Models.OnBoardingRequestResposeModel;
using F4ConversationCloud.Domain.Entities;
using F4ConversationCloud.Domain.Enum;


namespace F4ConversationCloud.Application.Common.Interfaces.Repositories
{
    public interface IAuthRepository
    {
        Task<int> CreateUserAsync(RegisterUserModel command);
        Task<int> InsertOTPAsync(VarifyMobileNumberModel command);
        Task<int> VerifyOTPAsync(ValidateRegistrationOTPModel oTPCommand);
        Task<int> UpdateClientFormStageAsync(int UserId, ClientFormStage Stageid);
        Task<int> CheckMailOrPhoneNumberAsync(VarifyMobileNumberModel command);

        Task <int> InsertMetaUsersConfigurationAsync(MetaUsersConfiguration command);
        Task<UserDetailsViewModel> GetCustomerById(int userId);

        Task<LoginViewModel> ValidateClientCreadiatial(string email);
        Task<IEnumerable<TimeZoneResponse>> GetTimeZonesAsync();

        Task<ResetPasswordViewModel> ValidateEmailId(string ClientEmailId);

        Task<int> UpdatePasswordAsync(ConfirmPasswordModel model);


    }
}
