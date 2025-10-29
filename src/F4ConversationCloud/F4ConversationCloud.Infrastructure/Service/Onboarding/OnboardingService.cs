using F4ConversationCloud.Application.Common.Interfaces.Repositories.Onboarding;
using F4ConversationCloud.Application.Common.Interfaces.Services;
using F4ConversationCloud.Application.Common.Interfaces.Services.Meta;
using F4ConversationCloud.Application.Common.Interfaces.Services.Onboarding;
using F4ConversationCloud.Application.Common.Meta.BussinessProfile;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.OnBoardingRequestResposeModel;
using F4ConversationCloud.Domain.Entities;
using F4ConversationCloud.Domain.Enum;
using F4ConversationCloud.Domain.Extension;
using F4ConversationCloud.Domain.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Twilio.Types;


namespace F4ConversationCloud.Application.Common.Services
{
    public class OnboardingService:IOnboardingService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IMessageService _messageService;
        private readonly IUrlHelper _urlHelper;
        private readonly IF4AppCloudeService _whatsAppCloude;

        public OnboardingService(IAuthRepository authRepository,IMessageService messageService, IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor,IF4AppCloudeService whatsAppCloudeService)
        {
            _authRepository = authRepository;  
            _messageService = messageService;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
            _whatsAppCloude = whatsAppCloudeService;

        }

        public async Task<VarifyUserDetailsResponse> CheckIsMailExitsAsync(VarifyMobileNumberModel request)
        {
            try
            {
                int ismailExit = await _authRepository.IsMailExitAsync(request);
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
                        return new VarifyUserDetailsResponse
                        {
                            status = false,
                            message = "Email already exists"

                        };
                }
                
                
             
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
                    status = sendMail? true:false,
                    message = sendMail ? "Email sent successfully" : "Failed to send email"
                };
           
                
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

        public async Task<VarifyUserDetailsResponse> VarifyWhatsAppContactNoAsync(VarifyMobileNumberModel request)
        {
            try
            {
              //int ContactNoExit = await _authRepository.IsContactNoExitAsync(request);
                            var CreateOTP = OtpGenerator.GenerateRandomOTP();
                                    //if (ContactNoExit != 0)
                                    //{
                                    //    return new VarifyUserDetailsResponse
                                    //    {
                                    //        status = false,
                                    //        message = "Already Registered With this Number!"

                                    //    };

                                    //}
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
                                        return new VarifyUserDetailsResponse
                                        {
                                            status = false,
                                            message = "Failed generate OTP"
                                        };
                                    }
                                    var sendWhatsAppOTP = await _messageService.SendOnboardingVerificationAsync(varificationRequest);
                                    if (string.IsNullOrEmpty(sendWhatsAppOTP.MessageId))
                                    {
                                        return new VarifyUserDetailsResponse
                                        {
                                            status = false,
                                            message = "Failed to send OTP via WhatsApp"
                                        };
                                    }

                return new VarifyUserDetailsResponse
                        {
                            status = true,
                            message = "OTP sent successfully to your WhatsApp.!"
                        };
                    
                
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



        public async Task<MetaUsersConfigurationResponse> InsertMetaUsersConfigurationAsync(MetaUsersConfiguration request)
        {
            try
            {
                if (!string.IsNullOrEmpty(request.PhoneNumberId))
                {
                    var businessInfo = await _whatsAppCloude.GetWhatsAppPhoneNumberDetailsAsync(request.PhoneNumberId);
                    var registerPhoneNumber = await _whatsAppCloude.RegisterClientAccountAsync( new ActivateClientAccountRequest { PhoneNumberId = request.PhoneNumberId });



                    string category = businessInfo?.WhatsAppBusinessProfile?.Data?.FirstOrDefault()?.Vertical;
                    string email = businessInfo?.WhatsAppBusinessProfile?.Data?.FirstOrDefault()?.Email;
                    string websites = businessInfo?.WhatsAppBusinessProfile?.Data?.FirstOrDefault()?.Websites[0];

                    var insertConfig = new MetaUsersConfiguration
                    {
                        ClientInfoId = request.ClientInfoId,
                        WabaId = request.WabaId,
                        PhoneNumberId = request.PhoneNumberId,
                        BusinessId = request.BusinessId,
                        WhatsAppBotName = businessInfo.VerifiedName,
                        Status = registerPhoneNumber.status,
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
                        return new MetaUsersConfigurationResponse
                        {
                            status = true,
                            Message = "Meta User Configuration Inserted Successfully"
                        };
                    }
                }

                return new MetaUsersConfigurationResponse
                {
                    status = false,
                    Message = "Meta User Configuration Insertion Failed"
                };
            }
            catch (Exception)
            {
                return new MetaUsersConfigurationResponse
                {
                    status = false,
                    Message = "Technical Error"
                };
            }
        }


        public async Task<RegisterUserResponse> RegisterUserAsync(RegisterUserModel request )
        {
            try
            {
                
                var register = await _authRepository.UpdateClientDetailsAsync(request);

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
                        Message = "User Updated successfully",
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
                        message = "OTP is Invalid or expired . Please check and Resend"
                    };
                }
            }
            catch (Exception ex)
            {
               
                return new ValidateRegistrationOTPResponse
                {
                    status = false,
                    message = "Technical Error!"
                };
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
                var loginUrl = _urlHelper.Action(
                         "Login",
                         "Onboarding",
                         null,
                         "https"
                     );
                var emailRequest = new EmailRequest
                {
                    ToEmail = request.UserEmailId,
                    Subject = "Your Meta Business Account Onboarding is Complete – Pending Admin Approval",
                    Body = "<p>Dear Customer,</p><br />" +
                           "Thank you for completing your Meta Business Account onboarding process. 🎉 <br/>" +
                           "Your account setup has been successfully submitted and is now pending admin approval.<br/>" +
                           "Our team will review your details shortly. Once approved, you will receive a confirmation email, and you can start managing your Meta Business Account without restrictions.<br/>" +
                           "Please wait while your account is reviewed.<br/>" +
                           "You’ll be notified via email once approval is granted.<br/><br/>" +
                           "<b>For login, please use the following link:</b><br/>" +
                          $"<a href=\"{loginUrl}\">Onboarding Login</a><br/><br/>" +
                           "Best regards,"

                };
                bool sendMail = await _messageService.SendEmail(emailRequest);

                return sendMail;
            }
            catch (Exception)
            {

                return false;
            }
        }
        public async Task SendRegistrationSuccessEmailAsync(RegisterUserModel Request)
        {
            try
            {

                //var loginUrl = _urlHelper.Action(
                //        "Login",      
                //        "Onboarding",  
                //        null,          
                //        "https"        
                //    );



                EmailRequest emailRequest = new EmailRequest()
                {
                    ToEmail = Request.Email,
                    Subject = "Your Fortune4 Registrations Completed – Pending Meta Onboarding",
                    Body = "<p>Dear Customer,</p><br />" +
                           "Thank you for completing your Fortune4 Registrations onboarding process. 🎉 <br/>" +
                           "Your account setup has been successfully submitted and is now pending Meta Registration.<br/>" +
                           //"To complete your Meta Registration, please use the link below:<br/><br/>" +
                           //$"<a href=\"{loginUrl}\">Onboarding Login</a><br/><br/>" +
                           "Best regards,"

                };
                await _messageService.SendEmail(emailRequest);
            }
            catch (Exception)
            {

                
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

        public async Task<bool> ValidateClientEmailAsync(string EmailId)
        {
            try
            {
                var userDetails = await _authRepository.ValidateEmailId(EmailId);

                if (userDetails is null)
                    return false;

                return true;

            }
            catch (Exception)
            {

               return false;
            }
           
        }

        public async Task SendResetPasswordLink(string EmailId)
        {
            try
            {
                var ClientDetails = await _authRepository.ValidateEmailId(EmailId);
                string id = ClientDetails.Id.ToString();
                string expiryTime = DateTime.UtcNow.AddMinutes(20).ToString();
                string token = (id + "|" + expiryTime).Encrypt().Replace("/", "thisisslash").Replace("\\", "thisisbackslash").Replace("+", "thisisplus");

                var resetUrl = _urlHelper.Action(
                             "ConfirmPassword",      
                             "Onboarding",           
                             new { id = token },     
                             "https"               
                         );


                EmailRequest email = new EmailRequest()
                {
                    ToEmail = ClientDetails.Email,
                    Subject = "Password Reset",
                    Body = "<h3>You can reset your password using the below link.</h3><br/>" +
                                $"<a href=\"{resetUrl}\">Click Here</a>" +
                               "<br/>Please note: This link will expire in 20 minutes."


                }; 
                await _messageService.SendEmail(email);
              
            }
            catch (Exception)
            {

               
            }
            

        }


        public async Task<bool> SetNewPassword(ConfirmPasswordModel model)
        {
            try
            {
                var UpdateRequest = new ConfirmPasswordModel
                {
                    UserId = model.UserId,
                    Password = PasswordHasherHelper.HashPassword(model.Password),

                };


                int result = await _authRepository.UpdatePasswordAsync(UpdateRequest);

                return result is not 0;

            }
            catch (Exception)
            {

               return false;
            }
           
        }


       
    }
}
