using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Interfaces.Services
{
    public interface ISendTemplateMessageService
    {
        Task SendTemplateMessageAsync(string userId, string templateName, Dictionary<string, string> parameters);
        Task SendTextMessageAsync(string userId, string message);
        Task<dynamic> MetaSendMessageTemplateAPICall(object requestBody, string PhoneID = null);
        Dictionary<string, string> BuildParameterDictionaryFromObject(List<string> parameterKeys, Object Details);
        Task SendOfferCarouselAsync(string userId, string TemplateName);
    }
}
