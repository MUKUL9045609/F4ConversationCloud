using F4ConversationCloud.Application.Common.Interfaces.Repositories.SuperAdmin;
using F4ConversationCloud.Application.Common.Interfaces.Services;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
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
        private readonly ILogService _logService;
        public SuperAdminAuthService(ISuperAdminAuthRepository superAdminAuthRepository, IEmailSenderService emailService, IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor,
            IWebHostEnvironment env, IConfiguration configuration, ILogService logService)
        {
            _superAdminAuthRepository = superAdminAuthRepository;
            _emailService = emailService;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
            _env = env;
            _configuration = configuration;
            _logService = logService;
        }

        public async Task<Auth> CheckUserExists(string username)
        {
            var response = new Auth();
            try
            {
                response = await _superAdminAuthRepository.CheckUserExists(username);
            }
            catch (Exception ex)
            {
                var logModel = new LogModel();
                logModel.Source = "SuperAdminAuth/CheckUserExists";
                logModel.AdditionalInfo = $"UserName: {username}";
                logModel.LogType = "Error";
                logModel.Message = ex.Message;
                logModel.StackTrace = ex.StackTrace;
                await _logService.InsertLogAsync(logModel);
            }
            finally
            {

            }
            return response;
        }

        public async Task<bool> ValidateUserName(string userName)
        {
            bool response = false;
            try
            {
                var userDetails = await _superAdminAuthRepository.CheckUserExists(userName);
                if (userDetails is null)
                    response = false;
            }
            catch (Exception ex)
            {
                var model = new LogModel();
                model.Source = "SuperAdminAuth/ValidateUserName";
                model.AdditionalInfo = $"UserName: {userName}";
                model.LogType = "Error";
                model.Message = ex.Message;
                model.StackTrace = ex.StackTrace;
                await _logService.InsertLogAsync(model);
            }
            finally
            {

            }
            return response;
        }

        public async Task<bool> SendPasswordResetLink(string userName)
        {
            var model = new LogModel();
            model.Source = "SuperAdminAuth/SendPasswordResetLink";
            model.AdditionalInfo = $"UserName: {userName}";
            bool response = false;
            try
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

                response = await _emailService.Send(emailRequest);
                model.LogType = "Success";
                model.Message = "Email sent successfully.";
            }
            catch (Exception ex)
            {
                model.LogType = "Error";
                model.Message = ex.Message;
                model.StackTrace = ex.StackTrace;
            }
            finally
            {
                await _logService.InsertLogAsync(model);
            }
            return response;
        }

        public async Task<bool> ConfirmPassword(ConfirmPasswordModel model)
        {
            bool response = false;
            try
            {
                int result = await _superAdminAuthRepository.UpdatePasswordAsync(new ConfirmPasswordModel { UserId = model.UserId, Password = model.Password.Encrypt() });

                response = result is not 0;
            }
            catch (Exception ex)
            {
                var logModel = new LogModel();
                logModel.Source = "SuperAdminAuth/ConfirmPassword";
                logModel.AdditionalInfo = $"Model: {model}";
                logModel.LogType = "Error";
                logModel.Message = ex.Message;
                logModel.StackTrace = ex.StackTrace;
                await _logService.InsertLogAsync(logModel);
            }
            finally
            {

            }
            return response;
        }

        public async Task<bool> CheckValidRole(IEnumerable<string> roles, string email)
        {

            bool response = false;
            try
            {
                var userDetails = await CheckUserExists(email);

                if (userDetails is null)
                {
                    response = false;
                }
                response = roles.Contains(userDetails.RoleName);
            }
            catch (Exception ex)
            {
                var logModel = new LogModel();
                logModel.Source = "SuperAdminAuth/CheckValidRole";
                logModel.AdditionalInfo = $"Roles: {string.Join(",", roles)}, Email: {email}";
                logModel.LogType = "Error";
                logModel.Message = ex.Message;
                logModel.StackTrace = ex.StackTrace;
                await _logService.InsertLogAsync(logModel);
            }
            finally
            {

            }
            return response;
        }
    }
}
