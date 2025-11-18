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
using Microsoft.AspNetCore.Mvc.Rendering;
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
        [HttpGet("template-list")]
        public async Task<IActionResult> List(TemplatesListViewModel request)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");

                var filter = new WhatsappTemplateListFilter
                {
                    ClientInfoId = Convert.ToInt32(userId),
                    TemplateName = request.TemplateName,
                    Category = request.Category.HasValue ? (TemplateModuleType?)request.Category.Value : null,
                    LangCode = request.templateLanguages.HasValue?(TemplateLanguages?)request.templateLanguages.Value:null,
                    ModifiedOn = request.ModifiedOn,
                    TemplateStatus = request.TemplateStatus.HasValue ? (TemplateApprovalStatus?)request.TemplateStatus.Value : null,
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
                        Category = ((TemplateModuleType)Convert.ToInt32(t.Category)).GetDisplayName(),
                        LanguageCode = ((TemplateLanguages)Convert.ToInt32(t.LanguageCode)).GetDisplayName(),
                        ModifiedOn = t.ModifiedOn,
                        TemplateStatus = ((TemplateApprovalStatus)Convert.ToInt32(t.TemplateStatus)).GetDisplayName()


                    }).ToList(),
                    TemplateName = request.TemplateName,
                    Category = request.Category,
                    LanguageList = Enum.GetValues(typeof(TemplateLanguages)).Cast<TemplateLanguages>().Select(e => new SelectListItem { Value = ((int)e).ToString(), Text = e.GetDisplayName(), Selected = request.templateLanguages == e }),
                    ModifiedOn = request.ModifiedOn,
                    StatusList = Enum.GetValues(typeof(TemplateApprovalStatus)).Cast<TemplateApprovalStatus>().Select(e => new SelectListItem { Value = ((int)e).ToString(), Text = e.GetDisplayName(), Selected = request.TemplateStatus == e }),
                    CategoryList = Enum.GetValues(typeof(TemplateModuleType)).Cast<TemplateModuleType>().Select(e => new SelectListItem { Value = ((int)e).ToString(), Text = e.GetDisplayName(), Selected = request.Category == e }),
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalCount = templatesData.TotalCount
                };
    
                return View(templateResponse);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading templates.";
                return View(new TemplatesListViewModel());
            }
        }

        public async Task<IActionResult> TemplateDetailsById(int templateId)
        {
            try
            {
               var templateDetails = await _whatsAppTemplate.GetTemplateByIdAsync(templateId);
                    
                var templateModel = new TemplatesListViewModel
                {
                    templateDetail = templateDetails,
                    TemplateButtons = templateDetails.TemplateButtons


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
