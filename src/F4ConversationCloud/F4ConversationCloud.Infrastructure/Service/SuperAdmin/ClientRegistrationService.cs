using F4ConversationCloud.Application.Common.Interfaces.Repositories.SuperAdmin;
using F4ConversationCloud.Application.Common.Interfaces.Services;
using F4ConversationCloud.Application.Common.Interfaces.Services.Common;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.ClientModel;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Domain.Entities.SuperAdmin;
using F4ConversationCloud.Domain.Extension;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

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
            var logModel = new LogModel();
            logModel.Source = "ClientRegistration/CreateUpdateAsync";
            logModel.AdditionalInfo = $"Model: {JsonConvert.SerializeObject(clientRegistration)}";
            int response = 0;
            try
            {
                response = await _clientRegistrationRepository.CreateUpdateAsync(clientRegistration);
                logModel.LogType = "Success";
                logModel.Message = "Client pre-registered successfully";
            }
            catch (Exception ex)
            {
                logModel.LogType = "Error";
                logModel.Message = ex.Message;
                logModel.StackTrace = ex.StackTrace;
            }
            finally
            {
                await _logService.InsertLogAsync(logModel);
            }
            return response;
        }

        public async Task<Tuple<IEnumerable<ClientRegistrationListItemModel>, int>> GetFilteredRegistrations(ClientRegistrationListFilter filter)
        {
            Tuple<IEnumerable<ClientRegistrationListItemModel>, int> response = Tuple.Create(Enumerable.Empty<ClientRegistrationListItemModel>(), 0);
            try
            {
                response = Tuple.Create(await _clientRegistrationRepository.GetFilteredAsync(filter), await _clientRegistrationRepository.GetCountAsync(filter));
            }
            catch (Exception ex)
            {
                var logModel = new LogModel();
                logModel.Source = "ClientRegistration/GetFilteredRegistrations";
                logModel.AdditionalInfo = $"Model: {JsonConvert.SerializeObject(filter)}";
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

        public async Task<ClientRegistration> GetByIdAsync(int id)
        {
            var response = new ClientRegistration();
            try
            {
                response = await _clientRegistrationRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                var logModel = new LogModel();
                logModel.Source = "ClientRegistration/GetByIdAsync";
                logModel.AdditionalInfo = $"Id: {id}";
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

        public async Task SendRegistrationEmailAsync(string email, string name, int id, string contactNumber)
        {
            var model = new LogModel();
            model.Source = "ClientRegistration/SendRegistrationEmailAsync";
            model.AdditionalInfo = $"Email: {email}, Name: {name}, ID: {id}";
            try
            {
                string templatePath = Path.Combine(_env.WebRootPath, "Html", "RegistrationEmailTemplate.html");
                string htmlBody = await File.ReadAllTextAsync(templatePath);

                var request = _httpContextAccessor.HttpContext.Request;
                string baseUrl = $"{request.Scheme}://{request.Host}";

                // Generate token with expiry
                var expiry = DateTime.UtcNow.AddHours(48);
                var tokenData = $"{id}|{expiry:O}";
                var encryptedToken = tokenData.Encrypt();

                string registrationLink = $"{_configuration["OnboardingUrl"]}?token={Uri.EscapeDataString(encryptedToken)}";

                string logo = $"{_configuration["MailerLogo"]}";
                string registrationImage = $"{_configuration["MailerRegistrationImage"]}";
                string currentYear = DateTime.Now.Year.ToString();
                htmlBody = htmlBody.Replace("{user_name}", name)
                                   .Replace("{Logo}", logo)
                                   .Replace("{RegistrationImage}", registrationImage)
                                   .Replace("{email}", email)
                                   .Replace("{contact_number}", contactNumber)
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
            bool response = false;
            try
            {
                response = await _clientRegistrationRepository.CheckEmailExist(email);
            }
            catch (Exception ex)
            {
                response = true;
                var logModel = new LogModel();
                logModel.Source = "ClientRegistration/CheckEmailExist";
                logModel.AdditionalInfo = $"Email: {email}";
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

        public async Task<RegisteredBusinessDetail> GetRegisteredBusinessDetail(int id)
        {
            var response = new RegisteredBusinessDetail();
            try
            {
                response = await _clientRegistrationRepository.GetRegisteredBusinessDetail(id);
            }
            catch (Exception ex)
            {
                var logModel = new LogModel();
                logModel.Source = "ClientRegistration/GetRegisteredBusinessDetail";
                logModel.AdditionalInfo = $"RegisteredId: {id}";
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

        public async Task<bool> CheckContactNumberExist(string contactNumber)
        {
            bool response = false;
            try
            {
                response = await _clientRegistrationRepository.CheckContactNumberExist(contactNumber);
            }
            catch (Exception ex)
            {
                response = true;
                var logModel = new LogModel();
                logModel.Source = "ClientRegistration/CheckContactNumberExist";
                logModel.AdditionalInfo = $"ContactNumber: {contactNumber}";
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


        public async Task<CommonSuperAdminServiceResponse> ActivateDeactivateClientAccountAsync(int clientId, int IsActive)
        {
            try
            {
                var tempRes = await _clientRegistrationRepository.ActivateDeactivateClientAccountAsync(clientId, IsActive);

                if (tempRes == 0)
                    return new CommonSuperAdminServiceResponse
                    {
                        success = false,
                        message = "Failed to Disable Client Account."
                    };

                return new CommonSuperAdminServiceResponse
                {
                    success = true,
                    message = "Client Account Disable successfully."
                };
            }
            catch (Exception ex)
            {
                var log = new LogModel
                {
                    Source = "WhatsappTemplate/DeactivateTemplateAsync",
                    AdditionalInfo = $"clientId: {clientId}",
                    LogType = "Error",
                    Message = ex.Message,
                    StackTrace = ex.StackTrace
                };

                await _logService.InsertLogAsync(log);


                return new CommonSuperAdminServiceResponse
                {
                    success = false,
                    message = "Technical Error."
                };
            }
        }
        public async Task<CommonSuperAdminServiceResponse> DeactivateClientAccountAsync(int clientId)
        {
            try
            {
                var tempRes = await _clientRegistrationRepository.ActivateClientAccountAsync(clientId);
                if (!tempRes)
                    return new CommonSuperAdminServiceResponse
                    {
                        success = false,
                        message = "Failed to Enable Client Account."
                    };

                return new CommonSuperAdminServiceResponse
                {
                    success = true,
                    message = "Client Account Enable successfully."
                };
            }
            catch (Exception ex)
            {
                var log = new LogModel
                {
                    Source = "WhatsappTemplate/ActivateClientAccountAsync",
                    AdditionalInfo = $"clientId: {clientId}",
                    LogType = "Error",
                    Message = ex.Message,
                    StackTrace = ex.StackTrace
                };

                await _logService.InsertLogAsync(log);


                return new CommonSuperAdminServiceResponse
                {
                    success = false,
                    message = "Technical Error."
                };
            }

        }

    }
}
