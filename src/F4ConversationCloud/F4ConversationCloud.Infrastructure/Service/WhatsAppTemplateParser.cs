using F4ConversationCloud.Application.Common.Interfaces.Services;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Infrastructure.Service
{
    public class WhatsAppTemplateParser : IWhatsAppTemplateParser
    {

        public WhatsAppTemplateData Parse(MessageTemplateDTO request)
        {
            var header = request.components?.FirstOrDefault(c => c?.@type?.Equals("header", StringComparison.OrdinalIgnoreCase) == true);
            var body = request.components?.FirstOrDefault(c => c?.@type?.Equals("body", StringComparison.OrdinalIgnoreCase) == true);
            var footer = request.components?.FirstOrDefault(c => c?.@type?.Equals("footer", StringComparison.OrdinalIgnoreCase) == true);

            string headerType = header?.@type ?? "";
            string headerFormat = header?.format ?? "";
            string headerText = "", headerExample = "", headerMediaUrl = "";

            if (headerFormat.Equals("text", StringComparison.OrdinalIgnoreCase))
            {
                headerText = header?.text ?? "";
                headerExample = string.Join(",", header?.example?.header_text ?? Array.Empty<string>());
            }
            else if (headerFormat.Equals("image", StringComparison.OrdinalIgnoreCase))
            {
                headerMediaUrl = string.Join(",", header?.example?.HeaderFile ?? Array.Empty<string>());
            }

            string bodyType = body?.@type ?? "";
            string bodyText = body?.text ?? "";
            string bodyExample = body?.example?.body_text is IEnumerable<IEnumerable<string>> examples
                ? string.Join(" | ", examples.Select(row => string.Join(",", row)))
                : "";

            string footerType = footer?.@type ?? "";
            string footerText = footer?.text ?? "";

            return new WhatsAppTemplateData(
                headerType, headerFormat, headerText, headerExample, headerMediaUrl,
                bodyType, bodyText, bodyExample,
                footerType, footerText
            );
        }

       
    
    }
}
