using F4ConversationCloud.Application.Common.Interfaces.Services;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Domain.Entities;
using F4ConversationCloud.Domain.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Infrastructure.Service
{
    public class WebhookService : IWebhookService
    {
        public WebhookService() { 
        
        }


        public async Task<dynamic> CallWebhookAsync(WhatsAppWebhookPayload requestBody)
        {
            string apiUrl = string.Empty;
            string methodType = "POST";
            var headers = new Dictionary<string, string>();

            try
            {
                apiUrl = ""; 

                string requestJson = JsonConvert.SerializeObject(requestBody);

                var result = await APICallingHelper.BindMainAPIRequestModel<dynamic, dynamic>(
                    apiUrl,
                    methodType,
                    requestBody,
                    headers,
                    "",
                    null,
                    false,
                    true
                );

                return result;
            }
            catch (Exception ex)
            {
                return new
                {
                    Success = false,
                    Error = ex.Message,
                    StackTrace = ex.StackTrace
                };
            }
        }

    }
}
