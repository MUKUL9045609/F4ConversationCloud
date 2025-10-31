using F4ConversationCloud.Application.Common.Interfaces.Repositories.SuperAdmin;
using F4ConversationCloud.Application.Common.Interfaces.Services;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Domain.Entities;
using F4ConversationCloud.Domain.Entities.SuperAdmin;
using F4ConversationCloud.Domain.Extension;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;

namespace F4ConversationCloud.Infrastructure.Service.SuperAdmin
{
    public class SuperAdminAuthService : ISuperAdminAuthService
    {
        private readonly ISuperAdminAuthRepository _superAdminAuthRepository;
        private readonly IEmailSenderService _emailService;
        private readonly IUrlHelper _urlHelper;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;
        public SuperAdminAuthService(ISuperAdminAuthRepository superAdminAuthRepository, IEmailSenderService emailService, IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor,
            IWebHostEnvironment env, IConfiguration configuration)
        {
            _superAdminAuthRepository = superAdminAuthRepository;
            _emailService = emailService;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
            _env = env;
            _configuration = configuration;
        }

        public async Task<Auth> CheckUserExists(string username)
        {
            return await _superAdminAuthRepository.CheckUserExists(username);
        }

        public async Task<bool> ValidateUserName(string userName)
        {
            var userDetails = await _superAdminAuthRepository.CheckUserExists(userName);

            if (userDetails is null)
                return false;

            return true;
        }

        public async Task SendPasswordResetLink(string userName)
        {
            var userDetails = await _superAdminAuthRepository.CheckUserExists(userName);

            string templatePath = Path.Combine(_env.WebRootPath, "Html", "PasswordResetEmailTemplate.html");
            string htmlBody = await File.ReadAllTextAsync(templatePath);

            string id = userDetails.Id.ToString();
            string expiryTime = DateTime.UtcNow.AddMinutes(20).ToString();
            string token = (id + "|" + expiryTime).Encrypt().Replace("/", "thisisslash").Replace("\\", "thisisbackslash").Replace("+", "thisisplus");

            var resetUrl = _urlHelper.Action("ConfirmPassword", "Auth", new { id = token }, "https");

            string logo = $"{_configuration["MailerLogo"]}";
            string lockImage = $"{_configuration["MailerLockImage"]}";
            string currentYear = DateTime.Now.Year.ToString();
            htmlBody = htmlBody.Replace("{user_name}", userDetails.FirstName + " " + userDetails.LastName)
                               .Replace("{Logo}", logo)
                               .Replace("{Lock_Image}", lockImage)
                               .Replace("{Link}", resetUrl)
                               .Replace("{CurrentYear}", currentYear);

            var emailRequest = new EmailRequest
            {
                ToEmail = userDetails.Email,
                Subject = "Password Reset",
                Body = htmlBody
            };

            await _emailService.Send(emailRequest);
        }

        public async Task<bool> ConfirmPassword(ConfirmPasswordModel model)
        {
            int result = await _superAdminAuthRepository.UpdatePasswordAsync(new ConfirmPasswordModel { UserId = model.UserId, Password = model.Password.Encrypt() });

            return result is not 0;
        }


        public async Task<bool> CheckValidRole(IEnumerable<string> roles, string email)
        {
            var userDetails = await CheckUserExists(email);

            if (userDetails is null)
            {
                return false;
            }

            return roles.Contains(userDetails.RoleName);
        }
    }
}
