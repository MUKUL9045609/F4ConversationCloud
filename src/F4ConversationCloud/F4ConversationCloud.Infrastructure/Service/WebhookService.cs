using F4ConversationCloud.Application.Common.Interfaces.Services;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Domain.Entities;
using F4ConversationCloud.Domain.Helpers;
using Newtonsoft.Json;

namespace F4ConversationCloud.Infrastructure.Service
{
    public class WebhookService : IWebhookService
    {
        private readonly IClientManagementService _clientManagement;
        public WebhookService(IClientManagementService clientManagement)
        {
            _clientManagement = clientManagement;
        }


        public async Task<dynamic> CallWebhookAsync(WhatsAppWebhookPayload requestBody)
        {
            string apiUrl = string.Empty;
            string methodType = "POST";
            var headers = new Dictionary<string, string>();

            try
            {
                
                var phonenumberId = requestBody.Entry[0].Changes[0].Value.Metadata.PhoneNumberId;
                var response =  _clientManagement.GetClientDetailsByPhoneNumberId(phonenumberId).Result;

                string requestJson = JsonConvert.SerializeObject(requestBody);

                var result = await APICallingHelper.BindMainAPIRequestModel<dynamic, dynamic>(
                    response.FirstOrDefault().WebHookUrl,
                    methodType,
                    requestBody.Entry[0],
                    headers,
                    "",
                    null,
                    false,
                    true
                );

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

