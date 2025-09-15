using F4ConversationCloud.Application.Common.Interfaces.Services;
using F4ConversationCloud.Application.Common.Models;
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

      


        public MessageService( IConfiguration configuration)
        {
            _configuration = configuration;
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




        public async Task<bool> SendEmail(EmailRequest Request)
        {
            try
            {
                string host = _configuration["MailSettings:SmtpServer"];
                int port = int.Parse(_configuration["MailSettings:SmtpPort"]);
                string user = _configuration["MailSettings:Username"];
                string password = _configuration["MailSettings:Password"];
                string senderEmail = _configuration["MailSettings:SenderEmail"];


                SmtpClient client = new SmtpClient(host, port)
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(user,password)
                };

                MailMessage mail = new MailMessage
                {
                    From = new MailAddress(senderEmail),
                    Subject = Request.Subject,
                    IsBodyHtml = true,
                    Body = Request.Body
                };

                mail.To.Add(Request.ToEmail);

                await client.SendMailAsync(mail);

                return true;
            }
            catch (Exception ex)
            {

                //  Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }


    }
}

