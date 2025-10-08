using F4ConversationCloud.Application.Common.Models.Templates;
using F4ConversationCloud.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Interfaces.Services
{
    public interface ITemplateService
    {
        Task<dynamic> CreateTemplate(MessageTemplate requestBody);
    }
}
