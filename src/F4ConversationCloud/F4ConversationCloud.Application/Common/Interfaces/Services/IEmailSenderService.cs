using F4ConversationCloud.Application.Common.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Interfaces.Services
{
    public interface IEmailSenderService
    {
         Task<bool> SendEmail(EmailRequest Request);












    }
}
