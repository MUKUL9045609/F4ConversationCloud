using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Interfaces.Services;
using F4ConversationCloud.Domain.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using F4ConversationCloud.Application.Common.Models.Templates;
using Twilio.Jwt.AccessToken;

namespace F4ConversationCloud.Infrastructure.Service
{
    public class TemplateService : ITemplateService
    {
        private readonly IAPILogService _logService;
        public TemplateService(IClientManagementService clientManagement, IAPILogService logService)
        {
           _logService = logService;
        }

        public async Task<dynamic> CreateTemplate(MessageTemplate requestBody)
        {
            string apiUrl = string.Empty;
            string methodType = "POST";
            var headers = new Dictionary<string, string>();

            try
            {
                string requestJson = JsonConvert.SerializeObject(requestBody);

                string token = "EAAqZAjK5EFEcBPBe6Lfoyi1pMh3cyrQbaBoyHvmLJeyMaZBnb8LsDPTxfdmAgZBcNZBQJpyOqwlQDMBTiMpmzrzZByRyHorE6U76Cffdf7KPzQZAxSEx7YZCMpZBZAN3wU9X1wTpYkrK0w6ZAHdE8SaKNU26js31LfrYB8dsJuQRF2stqwl26qKhJrLTOBUuTcygZDZD";
                headers = new Dictionary<string, string> { { "Authorization", $"Bearer {token}" } };


                var result = await _logService.CallExternalAPI<dynamic>("https://graph.facebook.com/v23.0/528970240291210/message_templates",
                                                                    methodType,
                                                                    requestBody,
                                                                    headers,
                                                                    "Create Template",
                                                                    null,
                                                                    true);

                return new
                {
                    Success = true,
                    Message = "Message Send SuccessFully."

                };
            }
            catch (Exception ex)
            {
                return new
                {
                    Message = "Message not send.",
                    Success = false,
                    Error = ex.Message,
                    StackTrace = ex.StackTrace
                };
            }
        }
    }


}
