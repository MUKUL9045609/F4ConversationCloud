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
using Microsoft.Extensions.Configuration;
using F4ConversationCloud.Application.Common.Interfaces.Services.Meta;

namespace F4ConversationCloud.Infrastructure.Service
{
    public class TemplateService : ITemplateService
    {
        private readonly IAPILogService _logService;
        private IConfiguration _configuration { get; }

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

                string token = _configuration["WhatsAppAPISettings:FacebookGraphMessageTemplatesEndpoint"];
                string WABAID = _configuration["WhatsAppAPISettings:FacebookGraphMessageTemplatesEndpoint"];

                headers = new Dictionary<string, string> { { "Authorization", $"Bearer {token}" } };

                var whatsAppBusinessAccountId = WABAID;

                var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.BaseAddress + WhatsAppBusinessRequestEndpoint.CreateTemplateMessage.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);

                var result = await _logService.CallExternalAPI<dynamic>(formattedWhatsAppEndpoint,
                                                                    methodType,
                                                                    requestBody,
                                                                    headers,
                                                                    "Create Template",
                                                                    null,
                                                                    true);

                return new
                {
                    Success = true,
                    Message = "Template created successFully."

                };
            }
            catch (Exception ex)
            {
                return new
                {
                    Message = "Template not created successFully.",
                    Success = false,
                    Error = ex.Message,
                    StackTrace = ex.StackTrace
                };
            }
        }
        public async Task<dynamic> EditTemplate(MessageTemplate requestBody , int TemplateID)
        {
            string apiUrl = string.Empty;
            string methodType = "POST";
            var headers = new Dictionary<string, string>();

            try
            {
                string requestJson = JsonConvert.SerializeObject(requestBody);

                string token = _configuration["WhatsAppAPISettings:FacebookGraphMessageTemplatesEndpoint"];
                string WABAID = _configuration["WhatsAppAPISettings:FacebookGraphMessageTemplatesEndpoint"];

                headers = new Dictionary<string, string> {{"Authorization", $"Bearer {token}" }};

                var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.BaseAddress + WhatsAppBusinessRequestEndpoint.TemplateID.Replace("{{TemplateID}}", TemplateID.ToString());

                var result = await _logService.CallExternalAPI<dynamic>(formattedWhatsAppEndpoint,
                                                                    methodType,
                                                                    requestBody,
                                                                    headers,
                                                                    "Edit Template",
                                                                    null,
                                                                    true);

                return new
                {
                    Success = true,
                    Message = "Template edited successFully."
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    Message = "Template not edited.",
                    Success = false,
                    Error = ex.Message,
                    StackTrace = ex.StackTrace
                };
            }
        }
        public async Task<dynamic> DeleteTemplate(int TemplateId, string TemplateName)
        {
            string apiUrl = string.Empty;
            string methodType = "DELETE";
            var headers = new Dictionary<string, string>();

            try
            {
         
                string token = _configuration["WhatsAppAPISettings:FacebookGraphMessageTemplatesEndpoint"];
                string WABAID = _configuration["WhatsAppAPISettings:FacebookGraphMessageTemplatesEndpoint"];

                headers = new Dictionary<string, string> { { "Authorization", $"Bearer {token}" } };

                var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GraphDeleteApiVersionBaseAddress.ToString().Replace("{{hsm_id}}", TemplateId.ToString()).Replace("{{name}}", TemplateName);

                var result = await _logService.CallExternalAPI<dynamic>(formattedWhatsAppEndpoint,
                                                                    methodType,
                                                                    null,
                                                                    headers,
                                                                    "Delete Template",
                                                                    null,
                                                                    true);

                return new
                {
                    Success = true,
                    Message = "Template deletd successFully."
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    Message = "Template not deletd.",
                    Success = false,
                    Error = ex.Message,
                    StackTrace = ex.StackTrace
                };
            }
        }
        public async Task<dynamic> DeleteTemplateByName(string TemplateName)
        {
            string apiUrl = string.Empty;
            string methodType = "DELETE";
            var headers = new Dictionary<string, string>();

            try
            {

                string token = _configuration["WhatsAppAPISettings:FacebookGraphMessageTemplatesEndpoint"];
                string WABAID = _configuration["WhatsAppAPISettings:FacebookGraphMessageTemplatesEndpoint"];

                headers = new Dictionary<string, string> { { "Authorization", $"Bearer {token}" } };

                var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GraphDeleteApiVersionBaseAddress.ToString().Replace("{{name}}", TemplateName);

                var result = await _logService.CallExternalAPI<dynamic>(formattedWhatsAppEndpoint,
                                                                    methodType,
                                                                    null,
                                                                    headers,
                                                                    "Delete Template",
                                                                    null,
                                                                    true);

                return new
                {
                    Success = true,
                    Message = "Template deletd successFully."
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    Message = "Template not deletd.",
                    Success = false,
                    Error = ex.Message,
                    StackTrace = ex.StackTrace
                };
            }
        }
    }


}
