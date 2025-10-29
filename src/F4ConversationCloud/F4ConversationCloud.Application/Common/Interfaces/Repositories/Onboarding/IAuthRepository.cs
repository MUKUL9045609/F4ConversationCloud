using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.OnBoardingModel;
using F4ConversationCloud.Application.Common.Models.OnBoardingRequestResposeModel;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Domain.Entities;
using F4ConversationCloud.Domain.Enum;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Interfaces.Repositories.Onboarding
{
    public interface IAuthRepository
    {
        Task<int> UpdateClientDetailsAsync(RegisterUserModel command);
        Task<int> InsertOTPAsync(VarifyMobileNumberModel command);
        Task<int> VerifyOTPAsync(ValidateRegistrationOTPModel oTPCommand);
        Task<int> UpdateClientFormStageAsync(int UserId, ClientFormStage Stageid);
        Task<int> IsMailExitAsync(VarifyMobileNumberModel command);
        Task<int> IsContactNoExitAsync(VarifyMobileNumberModel command);
        Task<int> InsertMetaUsersConfigurationAsync(MetaUsersConfiguration command);
        Task<UserDetailsViewModel> GetCustomerById(int userId);

        Task<LoginViewModel> ValidateClientCreadiatial(string email);
        Task<IEnumerable<TimeZoneResponse>> GetTimeZonesAsync();
        Task<IEnumerable<Cities>> GetCitiesAsync();
        Task<IEnumerable<States>> GetStatesAsync();
        Task<IEnumerable<Cities>> GetCitiesByStatesIdAsync(int stateId);
        Task<ResetPasswordResponseViewModel> ValidateEmailId(string ClientEmailId);
        Task<int> UpdatePasswordAsync(ConfirmPasswordModel model);
        Task<int> GetRegisteredClientCountAsync();
        Task<ClientDetails> GetClientInfoByEmailId(UserDetailsDTO userDetailsDTO);


    }
}
