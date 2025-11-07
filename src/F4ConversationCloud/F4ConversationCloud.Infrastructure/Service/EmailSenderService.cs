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
                        Body = message.Body
                    };

                    mailMessage.To.Add(message.ToEmail);

                    await smtpClient.SendMailAsync(mailMessage);
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
