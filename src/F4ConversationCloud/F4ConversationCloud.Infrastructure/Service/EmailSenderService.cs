using F4ConversationCloud.Application.Common.Interfaces.Services;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Domain.Helpers;
using F4ConversationCloud.Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace F4ConversationCloud.Infrastructure.Service
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly IConfiguration _configuration;
        public EmailSenderService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendEmail(EmailRequest Request)
        {
            try
            {
                string host = "smtp.gmail.com";
                int port = 587;


                SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("ashwin.c@fortune4.in", "pltk hndi ptmz inub")
                };

                MailMessage mail = new MailMessage
                {
                    From = new MailAddress("ashwin.c@fortune4.in"),
                    Subject = Request.Subject,
                    IsBodyHtml = true,
                    Body = "<p>Dear Customer,</p><br />" +
                             "Thank you for signing up with us. To verify your email, please enter the following <br/>" +
                             "One Time Password (OTP): " + Request.OTP + " <br/>" +
                             "This OTP is valid for 10 minutes from the receipt of this email.<br/>Best regards"
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

        public async Task<bool> Send(EmailRequest message)
        {
            try
            {
                var emailSettings = _configuration.GetSection("EmailSettings").Get<EmailSettings>();

                using (var smtpClient = new SmtpClient(emailSettings.SmtpServer, emailSettings.SmtpPort))
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(emailSettings.Username, emailSettings.Password);
                    smtpClient.EnableSsl = true;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(emailSettings.SenderEmail),
                        Subject = message.Subject,
                        IsBodyHtml = true,
                        Body = message.Body.Replace("{BaseUrl}", _configuration["BaseUrl"])
                    };

                    mailMessage.To.Add(message.ToEmail);

                    smtpClient.Send(mailMessage);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
