using F4ConversationCloud.Application.Common.Interfaces.Services.Common;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models.CommonModels;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.ClientAdmin.Models.TemplateModel;
using F4ConversationCloud.Infrastructure.Service.Common;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace F4ConversationCloud.ClientAdmin.Controllers
{
    public class TemplateManagemetController : BaseController
    {
        private readonly IWhatsAppTemplateService _whatsAppTemplate;
        private readonly ILogService _logService;

        public TemplateManagemetController(IWhatsAppTemplateService whatsAppTemplate, ILogService logService)
        {
            _whatsAppTemplate = whatsAppTemplate;
            _logService = logService;
        }
        public async Task<IActionResult> List(TemplatesListViewModel model)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");

                var filter = new WhatsappTemplateListFilter
                {
                    ClientInfoId = Convert.ToInt32(userId),
                    TemplateName = model.TemplateName,
                    Category = model.Category,
                    LangCode = model.LangCode,
                    ModifiedOn = model.ModifiedOn,
                    TemplateStatus = model.TemplateStatus,
                    PageNumber = model.PageNumber,
                    PageSize = model.PageSize
                };

                var templatesData = await _whatsAppTemplate.GetTemplatesListAsync(filter);

                var templateResponse = new TemplatesListViewModel
                {
                    Templates = templatesData.Templates?.Select(t => new WhatsappTemplateListItem
                    {
                        SrNo = t.SrNo,
                        TemplateName = t.TemplateName,
                        Category = t.Category,
                        LanguageCode = t.LanguageCode,
                        ModifiedOn = t.ModifiedOn,
                        TemplateStatus = t.TemplateStatus
                    }).ToList(),
                    TotalCount = templatesData.TotalCount,
                    PageNumber = model.PageNumber,
                    PageSize = model.PageSize
                };

                return View(templateResponse);
            }
            catch (Exception ex)
            {
                var logModel = new LogModel
                {
                    Source = "WhatsappTemplate/List",
                    AdditionalInfo = $"Model: {JsonConvert.SerializeObject(model)}",
                    LogType = "Error",
                    Message = ex.Message,
                    StackTrace = ex.StackTrace
                };
                await _logService.InsertLogAsync(logModel);

                TempData["ErrorMessage"] = "An error occurred while loading templates.";
                return View(new TemplatesListViewModel());
            }
        }

    }

}
