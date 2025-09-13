using F4ConversationCloud.Application.Common.Models.OnBoardingRequestResposeModel;


namespace F4ConversationCloud.Application.Common.Interfaces.Repositories
{
    public interface IAuthRepository
    {
        Task<int> CreateUserAsync(RegisterUserModel command);
        Task<int> InsertOTPAsync(VarifyMobileNumberModel command);
        Task<int> VerifyOTPAsync(ValidateRegistrationOTPModel oTPCommand);

        Task<int> CheckMailOrPhoneNumberAsync(VarifyMobileNumberModel command);

        Task <int> InsertMetaUsersConfigurationAsync(MetaUsersConfiguration command);
        Task<UserDetailsViewModel> GetCustomerById(int userId);
    }
}
