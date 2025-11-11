using F4ConversationCloud.Application.Common.Models.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Interfaces.Repositories
{
    public interface ITemplateRepositories
    {
        Task<dynamic> MetaCreateTemplate(TemplateRequest requestBody);
        Task<dynamic> MetaEditTemplate(EditTemplateRequest requestBody);
    }
}
