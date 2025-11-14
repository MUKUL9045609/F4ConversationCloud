using F4ConversationCloud.Application.Common.Interfaces.Repositories.Onboarding;
using F4ConversationCloud.Application.Common.Interfaces.Services;
using F4ConversationCloud.Application.Common.Interfaces.Services.Common;
using F4ConversationCloud.Application.Common.Interfaces.Services.Meta;
using F4ConversationCloud.Application.Common.Interfaces.Services.Onboarding;
using F4ConversationCloud.Application.Common.Meta.BussinessProfile;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.ClientModel;
using F4ConversationCloud.Application.Common.Models.OnBoardingModel;
using F4ConversationCloud.Application.Common.Models.OnBoardingRequestResposeModel;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Domain.Entities;
using F4ConversationCloud.Domain.Enum;
using F4ConversationCloud.Domain.Extension;
using F4ConversationCloud.Domain.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection;
using Twilio.Types;


namespace F4ConversationCloud.Application.Common.Services
{
    public class OnboardingService:IOnboardingService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IMessageService _messageService;
        private readonly IUrlHelper _urlHelper;
        private readonly IF4AppCloudeService _whatsAppCloude;
        private readonly IMetaService _metaService;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;
        private readonly IEmailSenderService _emailService;
        private readonly ILogService _logService;
        public OnboardingService(IAuthRepository authRepository,IMessageService messageService, IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor,IF4AppCloudeService whatsAppCloudeService, IMetaService metaService, IWebHostEnvironment env,IConfiguration configuration, IEmailSenderService emailService,ILogService logService)
        {
            _authRepository = authRepository;  
            _messageService = messageService;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
            _whatsAppCloude = whatsAppCloudeService;
            _metaService = metaService;
            _env = env;
            _configuration = configuration;
            _emailService = emailService;
            _logService = logService;   
        }

        public async Task<VarifyUserDetailsResponse> CheckIsMailExitsAsync(VarifyMobileNumberModel request)
        {
            try
            {
                int ismailExit = await _authRepository.IsMailExitAsync(request);
                var CreateOTP = OtpGenerator.GenerateRandomOTP();
                if (ismailExit == 0)
                {
                    return new VarifyUserDetailsResponse
                    {
                        status = false,
                        message = "Email already exists"

                    };
                   
                }
                var InsertOTPCommand = new VarifyMobileNumberModel
                {
                    UserEmailId = request.UserEmailId,
                    UserPhoneNumber = request.UserPhoneNumber,
                    OTP = CreateOTP,
                    OTP_Source = request.OTP_Source
                };

                var insertOTPResponse = await _authRepository.InsertOTPAsync(InsertOTPCommand);
                if (insertOTPResponse == 0)
                {
                    return new VarifyUserDetailsResponse
                    {
                        status = false,
                        message = "Failed to generate OTP"
                    };
                }
                else
                {
                    return new VarifyUserDetailsResponse
                    {
                        status = true,
                        message = "OTP generated successfully"
                    };

                }
            }
            catch (Exception)
            {
                return new VarifyUserDetailsResponse
                {
                    status = false,
                    message = "Technical Error!"
                };
            }

        }

        public async Task<VarifyUserDetailsResponse> VarifyWhatsAppContactNoAsync(VarifyMobileNumberModel request)
        {
            var model = new OnBoardingLogsModel
            {
                Source = "Onboarding/VarifyWhatsAppContactNoAsync",
                AdditionalInfo = $"Request: {JsonConvert.SerializeObject(request)}"
            };

            try
            {
                var CreateOTP = OtpGenerator.GenerateRandomOTP();

                var varificationRequest = new VarifyMobileNumberModel
                {
                    UserEmailId = request.UserEmailId,
                    UserPhoneNumber = $"{request.CountryCode}{request.UserPhoneNumber}",
                    OTP = CreateOTP,
                    OTP_Source = "WhatsApp"
                };

                var insertOTPResponse = await _authRepository.InsertOTPAsync(varificationRequest);

                if (insertOTPResponse != 1)
                {
                    model.LogType = "Failure";
                    model.Message = "Failed to generate OTP.";
                    model.AdditionalInfo = $"InsertOTPResponse: {JsonConvert.SerializeObject(varificationRequest)}";
                    return new VarifyUserDetailsResponse
                    {
                        status = false,
                        message = "Failed to generate OTP"
                    };
                }

                var sendWhatsAppOTP = await _messageService.SendOnboardingVerificationAsync(varificationRequest);

                if (string.IsNullOrEmpty(sendWhatsAppOTP.MessageId))
                {
                    model.LogType = "Failure";
                    model.Message = "Failed to send OTP On WhatsApp.";
                    model.AdditionalInfo = $"SendWhatsAppResponse: {JsonConvert.SerializeObject(sendWhatsAppOTP)}";
                    return new VarifyUserDetailsResponse
                    {
                        status = false,
                        message = "Failed to send OTP via WhatsApp"
                    };
                }

                model.LogType = "Success";
                model.Message = "OTP sent successfully.";
                model.AdditionalInfo = $"OTPRequest: {JsonConvert.SerializeObject(varificationRequest)}";

                return new VarifyUserDetailsResponse
                {
                    status = true,
                    message = "OTP sent successfully to your WhatsApp.!"
                };
            }
            catch (Exception ex)
            {
                model.LogType = "Error";
                model.Message = ex.Message;
                model.StackTrace = ex.StackTrace;

                return new VarifyUserDetailsResponse
                {
                    status = false,
                    message = "Technical Error!"
                };
            }
            finally
            {
                await _logService.InsertOnboardingLogs(model);
            }
        }


        public async Task<MetaUsersConfigurationResponse> InsertMetaUsersConfigurationAsync(MetaUsersConfiguration request)
        {
            var model = new ClientAdminLogsModel
            {
                Source = "Onboarding/InsertMetaUsersConfigurationAsync",
                Data = $"Data: {JsonConvert.SerializeObject(request)}"
            };
            try
            {

                    if (string.IsNullOrEmpty(request.PhoneNumberId))
                    {
                        model.LogType = "Failure";
                        model.Message = "PhoneNumberId is missing.";
                        return new MetaUsersConfigurationResponse
                        {
                            status = false,
                            Message = "Phone number ID is required."
                        };
                    }
                    var registerPhoneNumber = await _metaService.RegisterPhone(new PhoneRegistrationOnMeta {PhoneNumberId = request.PhoneNumberId });
                    var businessInfo = await _whatsAppCloude.GetWhatsAppPhoneNumberDetailsAsync(request.PhoneNumberId);

                    string category = businessInfo?.WhatsAppBusinessProfile?.Data?.FirstOrDefault()?.Vertical;
                    string email = businessInfo?.WhatsAppBusinessProfile?.Data?.FirstOrDefault()?.Email;
                    string websites = businessInfo?.WhatsAppBusinessProfile?.Data?.FirstOrDefault()?.Websites?.FirstOrDefault();

                    var insertConfig = new MetaUsersConfiguration
                    {
                        ClientInfoId = request.ClientInfoId,
                        WabaId = request.WabaId,
                        PhoneNumberId = request.PhoneNumberId,
                        BusinessId = request.BusinessId,
                        WhatsAppBotName = businessInfo.VerifiedName,
                        Status = businessInfo.WhatsAppStatus,
                        PhoneNumber = businessInfo.DisplayPhoneNumber,
                        AppVersion = request.AppVersion,
                        ApprovalStatus = ClientRegistrationStatus.Active,
                        ClientEmail = email,
                        WebSite = websites,
                        Category = category,
                    };

                    var response = await _authRepository.InsertMetaUsersConfigurationAsync(insertConfig);

                    if (response > 0)
                    {
                        model.Data = $"Data: {JsonConvert.SerializeObject(insertConfig)}";
                        model.LogType = "Success";
                        model.Message = "Meta User Configuration Inserted Successfully";
                        return new MetaUsersConfigurationResponse
                        {
                            status = true,
                            Message = "Meta User Configuration Inserted Successfully"
                        };
                    }
                model.Data = $"Data: {JsonConvert.SerializeObject(insertConfig)}";
                model.LogType = "Failure";
                model.Message = "Meta User Configuration Insertion Failed";

                return new MetaUsersConfigurationResponse
                {

                    status = false,
                    Message = "Meta User Configuration Insertion Failed"
                };
            }
            catch (Exception ex)
            {
                model.LogType = "Error";
                model.Message = ex.Message;
                model.StackTrace = ex.StackTrace;
                return new MetaUsersConfigurationResponse
                {
                    status = false,
                    Message = "Technical Error"
                };
            }
            finally
            {
                await _logService.InsertClientAdminLogsAsync(model);
            }
        }

        public async Task<RegisterUserResponse> RegisterUserAsync(RegisterUserModel request )
        {
            var model = new OnBoardingLogsModel
            {
                Source = "Onboarding/RegisterUserAsync",
                AdditionalInfo = $"Model: {JsonConvert.SerializeObject(request)}"
            };
            try
            {
                
                var register = await _authRepository.UpdateClientDetailsAsync(request);

                if (register <= 0)
                {
                    model.LogType = "Failure";
                    model.Message = "Fail to create user";
                    return new RegisterUserResponse
                    {
                        Message = "Fail to create user",
                        IsSuccess = false
                    };
                }
                else
                {
                    model.LogType = "Success";
                    model.Message = "User Updated successfully";
                    return new RegisterUserResponse
                    {
                        Message = "User Updated successfully",
                        IsSuccess = true,
                        NewUserId = register
                    };
                }

            }
            catch (Exception ex)
            {
                model.LogType = "Error";
                model.Message = ex.Message;
                model.StackTrace = ex.StackTrace;
                return new RegisterUserResponse
                {
                    Message = "technical error",
                    IsSuccess = false
                };
            }
            finally
            {
                await _logService.InsertOnboardingLogs(model);
            }
        }

        public async Task<ValidateRegistrationOTPResponse> VerifyOTPAsync(ValidateRegistrationOTPModel request)
        {
            var model = new OnBoardingLogsModel
            {
                Source = "Onboarding/VerifyOTPAsync",
                AdditionalInfo = $"Model: {JsonConvert.SerializeObject(request)}"
            };
            try
            {
                var verifyOtpRequest = new ValidateRegistrationOTPModel
                {
                    OTP = request.OTP,
                    UserPhoneNumber = $"{request.CountryCode}{request.UserPhoneNumber}",
                };

                var checkOTP = await _authRepository.VerifyOTPAsync(verifyOtpRequest);

                if (checkOTP > 0)
                {

                    return new ValidateRegistrationOTPResponse
                    {
                        status = true,
                        message = " OTP verified successfully!"
                    };
                }
                else
                {

                    return new ValidateRegistrationOTPResponse
                    {
                        status = false,
                        message = "OTP is Invalid or expired . Please check and Resend After 2 Min"
                    };
                }
            }
            catch (Exception ex)
            {
                model.LogType = "Error";
                model.Message = ex.Message;
                model.StackTrace = ex.StackTrace;
               
                return new ValidateRegistrationOTPResponse
                {
                    status = false,
                    message = "Technical Error!"
                };
            }
            finally {

                await _logService.InsertOnboardingLogs(model);
            }
            
        }

        public async Task<RegisterUserModel> GetCustomerByIdAsync(int UserId) {
            var model = new OnBoardingLogsModel
            {
                Source = "Onboarding/GetCustomerByIdAsync",
                AdditionalInfo = $"UserId: {UserId}"
            };
            try
            {
                var response = await _authRepository.GetCustomerById(UserId);

                return response;
            }
            catch (Exception ex)
            {
                model.LogType = "Error";
                model.Message = ex.Message;
                model.StackTrace = ex.StackTrace;
                await _logService.InsertOnboardingLogs(model);
                return null;
            }
        
        }

        public async Task<LoginResponse> ClientLogin(Loginrequest request) 
        {
            var log = new ClientAdminLogsModel
            {
                Source = "Onboarding/OnboardingLogin",
                Data = $"Model: {JsonConvert.SerializeObject(request)}",
            };

            try
            {

                var ClientDetails = await _authRepository.ValidateClientCreadiatial(request.Email);
                if (ClientDetails is null)
                {
                    return new LoginResponse
                    {
                        Message = "InValid Credential",
                        IsSuccess = false,
                        Data = null
                    };
                }
                return new LoginResponse()
                {
                    Message = "Login Success",
                    IsSuccess = true,
                    Data = new LoginViewModel
                    {
                        UserId = ClientDetails.UserId,
                        Email = ClientDetails.Email,
                        Stage = ClientDetails.Stage,
                        Password = ClientDetails.Password,
                    }
                };


            }
            catch (Exception ex)
            {
                log.LogType = "Error";
                log.Message = ex.Message;
                log.StackTrace = ex.StackTrace;
                return new LoginResponse
                {
                    Message = "Technical Error",
                    IsSuccess = false,
                };
            }
            finally {
                await _logService.InsertClientAdminLogsAsync(log);
            }


        }

        public async Task<bool> ValidateClientEmailAsync(string EmailId)
        {
            var Log = new ClientAdminLogsModel
            {
                Source = "Onboarding/ValidateClientEmailAsync",
                Data = $"EmailId: {EmailId}",
               
            };

            try
            {
                var userDetails = await _authRepository.ValidateEmailId(EmailId);

                if (userDetails is null)
                    return false;

                return true;

            }
            catch (Exception ex)
            {
                Log.LogType = "Error";
                Log.Message = ex.Message;
                Log.StackTrace = ex.StackTrace;

                return false;
            }
            finally {
                await _logService.InsertClientAdminLogsAsync(Log);
            }
           

        }

        public async Task SendResetPasswordLink(string EmailId)
        {
            var Log = new ClientAdminLogsModel
            {
                Source = "Onboarding/SendResetPasswordLink",
                Data = $"EmailId: {EmailId}"
            };
            try
            {
                var ClientDetails = await _authRepository.ValidateEmailId(EmailId);
                string templatePath = Path.Combine(_env.WebRootPath, "Html", "PasswordResetMailTemplate.html");
                string htmlBody = await File.ReadAllTextAsync(templatePath);


                string id = ClientDetails.Id.ToString();
                string expiryTime = DateTime.UtcNow.AddMinutes(20).ToString();
                string token = (id + "|" + expiryTime).Encrypt().Replace("/", "thisisslash").Replace("\\", "thisisbackslash").Replace("+", "thisisplus");

                var resetUrl = _urlHelper.Action("ConfirmPassword", "Auth", new { id = token }, "https");
                string logo = $"{_configuration["MailerLogo"]}";
                string lockImage = $"{_configuration["MailerLockImage"]}";
                string currentYear = DateTime.Now.Year.ToString();
                htmlBody = htmlBody.Replace("{user_name}", ClientDetails.FirstName + " " + ClientDetails.LastName)
                                   .Replace("{Logo}", logo)
                                   .Replace("{Lock_Image}", lockImage)
                                   .Replace("{Link}", resetUrl)
                                   .Replace("{CurrentYear}", currentYear);

                EmailRequest emailRequest = new EmailRequest()
                {
                    ToEmail = ClientDetails.Email,
                    Subject = "Password Reset",
                    Body = htmlBody
                };

                await _emailService.Send(emailRequest);
                Log.Data = $"EmailSendRequest: {emailRequest}";
                Log.LogType = "Success";
                Log.Message = "Password Reset email sent successfully";

            }
            catch (Exception ex)
            {
                Log.LogType = "Error";
                Log.Message = ex.Message;
                Log.StackTrace = ex.StackTrace;
            }
            finally {
                await _logService.InsertClientAdminLogsAsync(Log);
            }
            

        }

        public async Task<bool> SetNewPassword(ConfirmPasswordModel model)
        {

            var Log = new ClientAdminLogsModel
            {
                Source = "Onboarding/SetNewPassword",
                Data = $"Model: {JsonConvert.SerializeObject(model)}"
            };
            try
            {
               
                int result = await _authRepository.UpdatePasswordAsync(model);

                Log.Data = $"EmailSendRequest:{JsonConvert.SerializeObject(model)}";
                Log.LogType = "Success";
                Log.Message = "Password Reset email sent successfully";
                return result is not 0;
            }
            catch (Exception ex)
            {
                Log.LogType = "Error";
                Log.Message = ex.Message;
                Log.StackTrace = ex.StackTrace;
                return false;
            }
            finally {
                await _logService.InsertClientAdminLogsAsync(Log);
            }
           
        }

    }
}
