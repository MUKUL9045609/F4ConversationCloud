using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models
{
    public record WhatsAppTemplateData(
                 string HeaderType, string HeaderFormat, string HeaderText, string HeaderExample, string HeaderMediaUrl,
                 string BodyType, string BodyText, string BodyExample,
                 string FooterType, string FooterText);
}
