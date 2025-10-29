using F4ConversationCloud.Application.Common.Interfaces.Repositories.SuperAdmin;
using F4ConversationCloud.Application.Common.Interfaces.Services;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Domain.Entities.SuperAdmin;
using F4ConversationCloud.Domain.Extension;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace F4ConversationCloud.Infrastructure.Service.SuperAdmin
{
    public class ClientRegistrationService : IClientRegistrationService
    {
        private readonly IClientRegistrationRepository _clientRegistrationRepository;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IEmailSenderService _emailSenderService;
        private readonly ILogService _logService;
        public ClientRegistrationService(IClientRegistrationRepository clientRegistrationRepository, IWebHostEnvironment env,
            IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IEmailSenderService emailSenderService, ILogService logService)
        {
            _clientRegistrationRepository = clientRegistrationRepository;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _emailSenderService = emailSenderService;
            _logService = logService;
        }

        public async Task<int> CreateUpdateAsync(ClientRegistration clientRegistration)
        {
            return await _clientRegistrationRepository.CreateUpdateAsync(clientRegistration);
        }

        //public async Task<int> CreateUpdateAsync(ClientRegistration clientRegistration)
        //{
        //    var logModel = new LogModel();
        //    logModel.Source = "ClientRegistration/Create";
        //    logModel.AdditionalInfo = $"Model: {clientRegistration}";
        //    int response = 0;
        //    try
        //    {
        //        response = await _clientRegistrationRepository.CreateUpdateAsync(clientRegistration);
        //        logModel.LogType = "Success";
        //        logModel.Message = "Client pre-registered successfully";
        //    }
        //    catch (Exception ex)
        //    {
        //        logModel.LogType = "Error";
        //        logModel.Message = ex.Message;
        //        logModel.StackTrace = ex.StackTrace;
        //    }
        //    finally
        //    {
        //        await _logService.InsertLogAsync(logModel);
        //    }
        //    return response;
        //}

        public async Task<Tuple<IEnumerable<ClientRegistrationListItemModel>, int>> GetFilteredRegistrations(ClientRegistrationListFilter filter)
        {
            return Tuple.Create(await _clientRegistrationRepository.GetFilteredAsync(filter), await _clientRegistrationRepository.GetCountAsync(filter));
        }

        public async Task<ClientRegistration> GetByIdAsync(int id)
        {
            ClientRegistration cr = await _clientRegistrationRepository.GetByIdAsync(id);

            if (cr is null) return null;

            return new ClientRegistration
            {
                Id = cr.Id,
                FirstName = cr.FirstName,
                LastName = cr.LastName,
                Email = cr.Email,
                ContactNumber = cr.ContactNumber,
                Role = cr.Role,
                RegistrationStatus = cr.RegistrationStatus
            };
        }

        public async Task SendRegistrationEmailAsync(string email, string name, int id)
        {
            var model = new LogModel();
            model.Source = "SendRegistrationEmailAsync";
            model.AdditionalInfo = $"Email: {email}, Name: {name}, ID: {id}";
            try
            {
                string templatePath = Path.Combine(_env.WebRootPath, "Html", "RegistrationEmailTemplate.html");
                string htmlBody = await File.ReadAllTextAsync(templatePath);

                var request = _httpContextAccessor.HttpContext.Request;
                string baseUrl = $"{request.Scheme}://{request.Host}";
                string encryptedId = id.ToString().Encrypt();

                string registrationLink = $"{_configuration["OnboardingUrl"]}?id={encryptedId}";
                string currentYear = DateTime.Now.Year.ToString();
                htmlBody = htmlBody.Replace("{user_name}", name)
                                   .Replace("{BaseUrl}", baseUrl)
                                   .Replace("{Link}", registrationLink)
                                   .Replace("{CurrentYear}", currentYear);

                var emailRequest = new EmailRequest
                {
                    ToEmail = email,
                    Subject = "Complete Your Registration",
                    Body = htmlBody
                };

                await _emailSenderService.Send(emailRequest);

                model.LogType = "Success";
                model.Message = "Registration email sent successfully.";
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
        }

        public async Task<bool> CheckEmailExist(string email)
        {
            return await _clientRegistrationRepository.CheckEmailExist(email);
        }
    }
}
