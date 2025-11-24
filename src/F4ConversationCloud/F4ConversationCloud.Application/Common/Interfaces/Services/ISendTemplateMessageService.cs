using F4ConversationCloud.Application.Common.Models.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Interfaces.Services
{
    public interface ISendTemplateMessageService
    {
        Task SendTemplateMessageAsync(SendTemplateDTO sendTemplateDTO);
        Task SendTextMessageAsync(string userId, string message);
        Task<dynamic> MetaSendMessageTemplateAPICall(object requestBody, string PhoneID = null);
        Dictionary<string, string> BuildParameterDictionaryFromObject(List<string> parameterKeys, Object Details);
        Task SendOfferCarouselAsync(string userId, string TemplateName);
    }
}
