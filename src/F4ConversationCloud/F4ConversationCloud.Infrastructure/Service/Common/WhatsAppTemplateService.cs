using F4ConversationCloud.Application.Common.Interfaces.Repositories.Common;
using F4ConversationCloud.Application.Common.Interfaces.Services.Common;
using F4ConversationCloud.Application.Common.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Rest.FlexApi.V1;

namespace F4ConversationCloud.Infrastructure.Service.Common
{
    public class WhatsAppTemplateService: IWhatsAppTemplateService
    {
        private readonly IWhatsAppTemplateRepository _templateRepository;
        private readonly ILogService _logService;
        public WhatsAppTemplateService(IWhatsAppTemplateRepository templateRepository, ILogService logService)
        {
            _templateRepository = templateRepository;
            _logService = logService;
        }

        public async Task<IEnumerable<WhatsappTemplateList>> GetTemplatesListAsync(WhatsappTemplateListFilter filter)
        {
            try
            {
              var templateList = await _templateRepository.GetTemplatesListAsync(filter);
                if(templateList == null || !templateList.Any())
                {
                    _logService.
                    _logService.LogInfo("No WhatsApp templates found with the provided filter.");
                }
                return await
            }
            catch (Exception)
            {

                return Enumerable.Empty<WhatsappTemplateList>();
            }
            
        }
    }
}
