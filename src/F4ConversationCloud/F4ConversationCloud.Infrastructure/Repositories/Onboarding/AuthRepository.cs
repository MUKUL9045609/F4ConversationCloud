using F4ConversationCloud.Infrastructure.Interfaces;
using Dapper;
using F4ConversationCloud.Application.Common.Models.OnBoardingModel;
using F4ConversationCloud.Application.Common.Models.OnBoardingRequestResposeModel;
using F4ConversationCloud.Domain.Entities;
using F4ConversationCloud.Domain.Enum;
using F4ConversationCloud.Application.Common.Interfaces.Repositories.Onboarding;
using System.Reflection.Metadata;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;


namespace F4ConversationCloud.Infrastructure.Repositories.Onboarding
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IGenericRepository<string> _repository;
        public AuthRepository(IGenericRepository<string> repository)
        {
            _repository = repository;
        }

        public async Task<int> CreateUserAsync(RegisterUserModel command)
        {
            try
            {
               
                    var nameParts = command.FirstName.Trim().Split(' ', 2); 
                        command.FirstName = nameParts[0];
                        command.LastName = nameParts.Length > 1 ? nameParts[1] : string.Empty;
               
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("FirstName", command.FirstName);
                parameters.Add("LastName", command.LastName);
                parameters.Add("Email", command.Email);
                parameters.Add("PassWord", command.PassWord);
                parameters.Add("ClientTimeZone", command.Timezone);
                parameters.Add("Stage", command.Stage);
                parameters.Add("PhoneNumber", command.FullPhoneNumber);
                parameters.Add("Address", command.Address);
                parameters.Add("Country", command.Country);
                parameters.Add("role", command.Role);
                parameters.Add("@ClientId", command.ClientId);
                var NewUserId =  await _repository.InsertUpdateAsync("[sp_RegisterNewUser]", parameters);

                return NewUserId;

            }
            catch (Exception)
            {

                return 0;
            }
           

        }

        public async Task<int> UpdateClientFormStageAsync(int UserId, ClientFormStage Stageid)
        {

            try
            {
                DynamicParameters parameters = new DynamicParameters();

                parameters.Add("UserId", UserId);
                parameters.Add("stageId", Stageid);

                return await _repository.InsertUpdateAsync("[sp_UpdateClientFormStage]", parameters);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<int> InsertOTPAsync(VarifyMobileNumberModel command)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
               
                parameters.Add("OTP", command.OTP);
                parameters.Add("@OTP_Source", command.OTP_Source);
                parameters.Add("@UserEmailId", command.UserEmailId);
                parameters.Add("@UserPhoneNumber", command.UserPhoneNumber);
                var response = await _repository.InsertUpdateAsync("[sp_InsertOTP]", parameters);
                return response;
            }
            catch (Exception)
            {
                return 0;
            }
        }


        public async Task<int> VerifyOTPAsync(ValidateRegistrationOTPModel oTPCommand)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("UserEmailId", oTPCommand.UserEmailId);
                parameters.Add("UserPhoneNumber", oTPCommand.UserPhoneNumber);
                parameters.Add("OTP", oTPCommand.OTP);
                var response = await _repository.GetByValuesAsync<int>("[sp_ValidateOTP]", parameters);
                return response;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public async Task<int> CheckMailOrPhoneNumberAsync(VarifyMobileNumberModel command)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("UserEmailId", command.UserEmailId);
                var response = await _repository.GetByValuesAsync<int>("[sp_CheckMailOrPhoneNumber]", parameters);
                return response;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<int> InsertMetaUsersConfigurationAsync(MetaUsersConfiguration command)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("WabaId", command.WabaId);
                parameters.Add("BusinessId", command.BusinessId);
                parameters.Add("PhoneNumberId", command.PhoneNumberId);
                parameters.Add("ClientInfoId", command.ClientInfoId);
                parameters.Add("WhatsAppBotName", command.WhatsAppBotName);
                parameters.Add("Status", command.Status);
                parameters.Add("PhoneNumber", command.PhoneNumber);
                parameters.Add("AppVersion", command.AppVersion);
                parameters.Add("@ApprovalStatus", command.ApprovalStatus);
                parameters.Add("ClientEmail", command.ClientEmail);
                parameters.Add("@WebSite", command.WebSite);
                parameters.Add("@Category", command.Category);

                return await _repository.InsertUpdateAsync("[dbo].[sp_InsertMetaUsersConfigurations]", parameters);
            }
            catch (Exception ex)
            {
               
                return 0;
            }
        }


        public async Task<UserDetailsViewModel> GetCustomerById(int userId)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("UserId", userId);
                var response = await _repository.GetByValuesAsync<UserDetailsViewModel>("[sp_GetCustomerById]", parameters);
                return response;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<LoginViewModel> ValidateClientCreadiatial(string Email)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@EmailId", Email);
                return await _repository.GetByValuesAsync<LoginViewModel>("[sp_ValidateClientCreadiatial]", parameters);
            }
            catch (Exception)
            {

              return await Task.FromResult<LoginViewModel>(null);
            }
           
        }

        public async Task<IEnumerable<TimeZoneResponse>> GetTimeZonesAsync()
        {
            DynamicParameters dp = new DynamicParameters();
            return await _repository.GetListByValuesAsync<TimeZoneResponse>("[sp_GetTimeZones]", dp);
        }

       
        public async Task<int> UpdatePasswordAsync(ConfirmPasswordModel model)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("id", model.UserId);
            parameters.Add("password", model.Password);

            return await _repository.UpdateValuesAsync("sp_UpdateClientPassword", parameters);
        }

        public Task<ResetPasswordResponseViewModel> ValidateEmailId(string ClientEmailId)
        {
            try
            {
                dynamic parameters = new DynamicParameters();
                parameters.Add("@email", ClientEmailId);
                return _repository.GetByValuesAsync<ResetPasswordResponseViewModel>("[sp_CheckUserExistsByMailId]", parameters);
            }
            catch (Exception)
            {

                return Task.FromResult<ResetPasswordResponseViewModel>(null);
            }

        }

        public async Task<int> sp_GetRegisteredClientCountAsync()
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                var response = await _repository.GetByValuesAsync<int>("[sp_GetRegisteredClientCount]", parameters);
                return response;
            }
            catch (Exception)
            {
                return 0;
            }
        }


        public async Task<ClientDetails> GetClientInfoByEmailId(UserDetailsDTO userDetailsDTO)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("EmailID", userDetailsDTO.EmailId);
                var response = await _repository.GetByValuesAsync<ClientDetails>("[sp_GetClientInfoByEmailId]", parameters);
                return response;
            }
            catch (Exception)
            {
                return new ClientDetails();
            }
        }


    }
}
