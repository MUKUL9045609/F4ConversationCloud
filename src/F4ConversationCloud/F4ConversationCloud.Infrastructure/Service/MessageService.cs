using F4ConversationCloud.Application.Common.Interfaces.Services;
using F4ConversationCloud.Application.Common.Interfaces.Services.Meta;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.MetaCloudApiModel.Templates;
using F4ConversationCloud.Application.Common.Models.OnBoardingModel;
using F4ConversationCloud.Application.Common.Models.OnBoardingRequestResposeModel;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Rest.Verify.V2.Service;
using Twilio.Types;
namespace F4ConversationCloud.Infrastructure.Service
{
    public class MessageService : IMessageService
    {
        private IConfiguration _configuration { get; }
        private IAPILogService _aPILogService { get; set; }
        private IMetaCloudAPIService _metaCloudAPIService;



        public MessageService( IConfiguration configuration,IMetaCloudAPIService metaCloudAPIService)
        {
            _configuration = configuration;
            _metaCloudAPIService = metaCloudAPIService;
        }
        public async Task<SendSmsResponse> SendVerificationSmsAsync(string mobileNo, string Text)
        {
            try 
            {
                string accountSid = _configuration["Twilio:AccountSid"];
                string authToken = _configuration["Twilio:AuthToken"];
                string verifyServiceSid = _configuration["Twilio:VerifyServiceSid"];
                string from = _configuration["Twilio:From"];
                TwilioClient.Init(accountSid, authToken);

                var messageOptions = new CreateMessageOptions(
                         new PhoneNumber(mobileNo)   
                     );
                messageOptions.Body = Text;
                messageOptions.From = new PhoneNumber(from);



                var message = await MessageResource.CreateAsync(messageOptions);

                var verification = await VerificationResource.CreateAsync(
                    to: mobileNo,
                    channel: "sms",
                    pathServiceSid: verifyServiceSid
                );

                return new SendSmsResponse
                {
                    Sid = verification.Sid,
                    Status = verification.Status
                };

            }
            catch (Exception ex)
            {
                return
                    new SendSmsResponse {
                        Sid = "",
                        Status = "Failed"
                    };
               
            }
        }

        public async Task<OnboardingContactNoVerificationResponse> SendOnboardingVerificationAsync(VarifyMobileNumberModel request)
        {
            try
            {
                var whatsappRequest = new TextTemplateMessageRequest
                {
                    To = request.UserPhoneNumber,
                    Template = new TextMessageTemplate
                    {
                        Name = "onboarding_contact_varification",
                        Language = new TextMessageLanguage
                        {
                            Code = "en_US"
                        },
                        Components = new List<TextMessageComponent>
                                {
                                    new TextMessageComponent
                                    {
                                        Type = "body",
                                        Parameters = new List<TextMessageParameter>
                                        {
                                            new TextMessageParameter
                                            {
                                                Type = "text",
                                                Text = request.OTP
                                            }
                                        }
                                    }
                                }
                    }
                };


             var whatsAppMessagereponse = await _metaCloudAPIService.SendTextMessageTemplateAsync(whatsappRequest); 

                return new OnboardingContactNoVerificationResponse
                {
                    MessageId = whatsAppMessagereponse != null && whatsAppMessagereponse.Messages != null && whatsAppMessagereponse.Messages.Count > 0 ? whatsAppMessagereponse.Messages[0].Id : null
                };

            }
            catch (Exception ex)
            {
                return
                    new OnboardingContactNoVerificationResponse
                    {
                    };

            }
        }
    }
}

