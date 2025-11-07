using F4ConversationCloud.Application.Common.Interfaces.Repositories.Common;
using F4ConversationCloud.Application.Common.Interfaces.Services.Common;
using F4ConversationCloud.Application.Common.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Infrastructure.Service.Common
{
    public class WhatsAppTemplateService: IWhatsAppTemplateService
    {
        private readonly IWhatsAppTemplateRepository _templateRepository;
        public WhatsAppTemplateService(IWhatsAppTemplateRepository templateRepository)
        {
            _templateRepository = templateRepository;
        }

        public async Task<IEnumerable<WhatsappTemplateList>> GetTemplatesListAsync(WhatsappTemplateListFilter filter)
        {
            try
            {
                return await _templateRepository.GetTemplatesListAsync(filter);
            }
            catch (Exception)
            {

                return Enumerable.Empty<WhatsappTemplateList>();
            }
            
        }
    }
}
