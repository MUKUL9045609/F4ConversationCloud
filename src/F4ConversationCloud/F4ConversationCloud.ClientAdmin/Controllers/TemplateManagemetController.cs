using F4ConversationCloud.Application.Common.Interfaces.Services.Common;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models.CommonModels;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.ClientAdmin.Models.TemplateModel;
using Microsoft.AspNetCore.Mvc;

namespace F4ConversationCloud.ClientAdmin.Controllers
{
    public class TemplateManagemetController : BaseController
    {
        private readonly IWhatsAppTemplateService _whatsAppTemplate;
        public TemplateManagemetController(IWhatsAppTemplateService whatsAppTemplate )
        {
            _whatsAppTemplate = whatsAppTemplate;
        }
        [HttpGet("List")]
        public async Task<IActionResult> List(TemplatesListViewModel model)
        {
            try
            {
                var filter = new WhatsappTemplateListFilter
                {
                    SearchText = model.SearchText,
                    PageNumber = model.PageNumber,
                    PageSize = model.PageSize,
                    LanguageCode = model.LanguageCode,
                    Status = model.Status,  
                };
                var templates = await _whatsAppTemplate.GetTemplatesListAsync(filter);
                return View(templates);
            }
            catch (Exception)
            {

              return View(Enumerable.Empty<WhatsappTemplateList>());
            }

        }
    }
}
