using F4ConversationCloud.Infrastructure.Interfaces;
using Dapper;
using F4ConversationCloud.Application.Common.Models.OnBoardingRequestResposeModel;
using F4ConversationCloud.Application.Common.Interfaces.Repositories.Onboarding;


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
                parameters.Add("CreatedBy", command.CreatedBy);
                parameters.Add("ModifedBy", command.ModifedBy);
                parameters.Add("PhoneNumber", command.PhoneNumber);
                parameters.Add("Address", command.Address);
                parameters.Add("Country", command.Country);
                parameters.Add("BankVarificationNumber", command.BankVarificationNumber);
                parameters.Add("role", command.Role);   
                var NewUserId =  await _repository.InsertUpdateAsync("[sp_RegisterNewUser]", parameters);

                return NewUserId;

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
                parameters.Add("UserPhoneNumber", command.UserPhoneNumber);
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
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("WabaId", command.WabaId);
                parameters.Add("AppName", command.AppName);
                parameters.Add("AccessToken", command.AccessToken);
                parameters.Add("AppVersion", command.AppVersion);
                parameters.Add("BusinessId", command.BusinessId);
                parameters.Add("CompanyName", command.CompanyName);
                parameters.Add("PhoneNumberId", command.PhoneNumberId);
                parameters.Add("OnboardingUserId", command.OnboardingUserId);


                return await _repository.InsertUpdateAsync("[sp_InsertMetaUsersConfigurations]", parameters);
            }
            catch (Exception)
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
    }
}
