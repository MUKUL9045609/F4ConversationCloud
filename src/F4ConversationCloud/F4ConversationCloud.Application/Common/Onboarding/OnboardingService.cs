
using F4ConversationCloud.Application.Common.Interfaces.IWebServices;
using F4ConversationCloud.Application.Common.Interfaces.Repositories;
using F4ConversationCloud.Application.Common.Interfaces.Services;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.OnBoardingRequestResposeModel;
using F4ConversationCloud.Domain.Helpers;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Data;


namespace F4ConversationCloud.Application.Common.Services
{
    public class OnboardingService:IOnboardingService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IMessageService _messageService;
        public OnboardingService(IAuthRepository authRepository,IMessageService messageService)
        {
            _authRepository = authRepository;  
            _messageService = messageService;
        }

        public async Task<VarifyUserDetailsResponse> CheckMailOrPhoneNumberAsync(VarifyMobileNumberModel request)
        {
            try
            {
                #region Insert OTP in DB if email or phone number not exist
                int ismailExit = await _authRepository.CheckMailOrPhoneNumberAsync(request);
                var CreateOTP = OtpGenerator.GenerateRandomOTP();
                if (ismailExit != 1)
                {
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
                }
                else
                {
                    if (!string.IsNullOrEmpty(request.UserEmailId))
                    {
                        return new VarifyUserDetailsResponse
                        {
                            status = false,
                            message = "Email already exists"

                        };
                    }
                    if (!string.IsNullOrEmpty(request.UserPhoneNumber))
                    {
                        return new VarifyUserDetailsResponse
                        {
                            status = false,
                            message = "Phone Number already exists"
                        };
                    }
                }
                #endregion
                #region Send OTP via Email
                if (string.IsNullOrEmpty(request.UserPhoneNumber))
                {
                    var emailRequest = new EmailRequest
                    {
                        ToEmail = request.UserEmailId,
                        Subject = "Your OTP Verification Code",
                        Body= "<p>Dear Customer,</p><br />" +
                             "Thank you for signing up with us. To verify your email, please enter the following <br/>" +
                             "One Time Password (OTP): " + CreateOTP + " <br/>" +
                             "This OTP is valid for 10 minutes from the receipt of this email.<br/>Best regards",
                       // OTP = CreateOTP,
                    };
                    bool sendMail = await _messageService.SendEmail(emailRequest);
                    return new VarifyUserDetailsResponse
                    {
                        status = sendMail,
                        message = sendMail ? "Email sent successfully" : "Failed to send email"
                    };
                }
                #endregion
                #region Send OTP via SMS
                if (string.IsNullOrEmpty(request.UserEmailId))
                {

                    // var sendSms = await _messageService.SendVerificationSmsAsync(request.UserPhoneNumber, "Your OTP Verification Code is " + CreateOTP);

                    /*  if (sendSms == null || sendSms.Status != "pending")
                      {
                          return new EmailSendResponse
                          {
                              status = false,
                              message = "Failed to send SMS"
                          };
                      }
                      */
                    return new VarifyUserDetailsResponse
                    {
                        /*status = await _emailService.SendSms(SmsRequest),*/
                        status = true,
                        message = "SMS sent successfully"
                    };
                }
                #endregion
                else
                {
                    return new VarifyUserDetailsResponse
                    {
                        status = false,
                        message = "Please provide either Email or Phone Number"
                    };
                }
                ;
            }
            catch (Exception)
            {
                return new VarifyUserDetailsResponse
                {
                    status = false,
                    message = "An error occurred while sending the email"
                };
            }

        }

        public async Task<MetaUsersConfigurationResponse> InsertMetaUsersConfigurationAsync(MetaUsersConfiguration request)
        {
            try
            {
                var response = await _authRepository.InsertMetaUsersConfigurationAsync(request);


                if (response > 0)
                {
                    return new MetaUsersConfigurationResponse
                    {
                        status = true,
                        Message = "Meta User Configuration Inserted Successfully"
                    };
                }
                else
                {
                    return new MetaUsersConfigurationResponse
                    {
                        status = false,
                        Message = "Meta User Configuration Insertion Failed"
                    };
                }
            }
            catch (Exception)
            {

                return new MetaUsersConfigurationResponse
                {
                    status = false,
                    Message = "Technical Error "
                };
            }
        }

        public async Task<RegisterUserResponse> RegisterUserAsync(RegisterUserModel request )
        {
            try
            {
                var registerRequest = new RegisterUserModel
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    
                    Address = request.Address,
                    Country = request.Country,
                    Timezone = request.Timezone,
                    PassWord = PasswordHasherHelper.HashPassword(request.PassWord),
                    IsActive = request.IsActive,
                    Stage = request.Stage,
                    FullPhoneNumber= request.FullPhoneNumber,
                    Role = request.Role,
                };

                var register = await _authRepository.CreateUserAsync(registerRequest);

                if (register <= 0)
                {
                    return new RegisterUserResponse
                    {
                        Message = "Fail to create user",
                        IsSuccess = false
                    };
                }
                else
                {

                    return new RegisterUserResponse
                    {
                        Message = "User created successfully",
                        IsSuccess = true,
                        NewUserId = register
                    };
                }

            }
            catch (Exception)
            {

                return new RegisterUserResponse
                {
                    Message = "technical error",
                    IsSuccess = false
                };
            }
        }

        

        public async Task<ValidateRegistrationOTPResponse> VerifyOTPAsync(ValidateRegistrationOTPModel request)
        {
            try
            {
                var checkOTP = await _authRepository.VerifyOTPAsync(request);

                if (checkOTP > 0)
                {
                    return new ValidateRegistrationOTPResponse { status = true };
                }
                else
                {
                    return new ValidateRegistrationOTPResponse { status = false };
                }
            }
            catch (Exception)
            {

                return new ValidateRegistrationOTPResponse { status = false };
            }
        }


       public async Task<UserDetailsViewModel> GetCustomerByIdAsync(int UserId) {
            try
            {
                var response = await _authRepository.GetCustomerById(UserId);

                return response;
            }
            catch (Exception)
            {

                return null;
            }
        
        }



        public async Task<bool> SendOnboardingConfirmationEmail(VarifyMobileNumberModel request)
        {
            try
            {
                var emailRequest = new EmailRequest
                {
                    ToEmail = request.UserEmailId,
                    Subject = "Your Meta Business Account Onboarding is Complete – Pending Admin Approval",
                    Body = "<p>Dear Customer,</p><br />" +
                                "Thank you for completing your Meta Business Account onboarding process. 🎉 <br/>"+
                                "Your account setup has been successfully submitted and is now pending admin approval.\r\nOur team will review your details shortly. Once approved, you will receive a confirmation email, and you can start managing your Meta Business Account without restrictions."+
                                "Please wait while your account is reviewed.\r\n"
                                + "You’ll be notified via email once approval is granted."
                                + "Best regards,"

                };
                bool sendMail = await _messageService.SendEmail(emailRequest);

                return sendMail;
            }
            catch (Exception)
            {

                return false;
            }
            

        }


        public async Task<LoginResponse> OnboardingLogin(Loginrequest request) 
        {

            try
            {
               
                 var ClientDetails = await _authRepository.ValidateClientCreadiatial(request.Email);
                    if (ClientDetails is null)
                        return new LoginResponse
                        {
                            Message = "InvalidEmail",
                            IsSuccess = false,

                        };
                    var isPasswordValid = PasswordHasherHelper.VerifyPassword(request.PassWord, ClientDetails.Password);
                        if (!isPasswordValid)
                        {
                            return new LoginResponse
                            {
                                Message = "InvalidPassword",
                                IsSuccess = false,
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
                    }
                };

            }
            catch (Exception)
            {
                return new LoginResponse
                {
                    Message = "Technical Error",
                    IsSuccess = false,
                };

            }


        }
    }
}
