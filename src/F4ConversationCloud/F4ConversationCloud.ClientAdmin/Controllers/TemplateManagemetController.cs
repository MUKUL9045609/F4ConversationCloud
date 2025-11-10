using BuldanaUrban.Domain.Helpers;
using F4ConversationCloud.Application.Common.Interfaces.Services.Common;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models.CommonModels;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.ClientAdmin.Models.TemplateModel;
using F4ConversationCloud.Domain.Enum;
using F4ConversationCloud.Domain.Extension;
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
        public async Task<IActionResult> List(TemplatesListViewModel request)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");

                var filter = new WhatsappTemplateListFilter
                {
                    ClientInfoId = Convert.ToInt32(userId),
                    TemplateName = request.TemplateName,
                    Category = request.Category,
                    LangCode = request.LangCode,
                    ModifiedOn = request.ModifiedOn,
                    TemplateStatus = request.TemplateStatus,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };

                var templatesData = await _whatsAppTemplate.GetTemplatesListAsync(filter);

                var templateResponse = new TemplatesListViewModel
                {
                    Templates = templatesData.Templates?.Select(t => new WhatsappTemplateListItem
                    {
                        SrNo = t.SrNo,
                        TemplateName = t.TemplateName,
                        TemplateId = t.TemplateId,
                        Category = t.Category,
                        LanguageCode = t.LanguageCode.ToString(),
                        ModifiedOn = t.ModifiedOn,
                        TemplateStatus = t.TemplateStatus
                    }).ToList(),
                    TemplateName = request.TemplateName,
                    Category = request.Category,
                    LangCode = request.LangCode,
                    ModifiedOn = request.ModifiedOn,
                    TemplateStatusSet = Enum.GetValues(typeof(TemplateApprovalStatus)).Cast<TemplateApprovalStatus>().ToDictionary(t => (int)t, t => t.GetDisplayName()),
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalCount= templatesData.TotalCount
                };

                return View(templateResponse);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading templates.";
                return View(new TemplatesListViewModel());
            }
        }

        public async Task<IActionResult> TemplateDetailsById(string templateId)
        {
            try
            {
               var templateDetails = await _whatsAppTemplate.GetTemplateByIdAsync(templateId);
                    
                var templateModel = new TemplatesListViewModel
                {
                    templateDetail = templateDetails
                };

                return PartialView("_TemplatePreviewModalPartial", templateModel);
            }
            catch (Exception)
            {

              return null;
            }
           
        }

    }

}
