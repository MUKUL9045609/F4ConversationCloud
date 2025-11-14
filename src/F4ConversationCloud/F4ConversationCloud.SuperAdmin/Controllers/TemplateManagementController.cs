using BuldanaUrban.Domain.Helpers;
using F4ConversationCloud.Application.Common.Interfaces.Repositories;
using F4ConversationCloud.Application.Common.Interfaces.Services.Common;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.CommonModels;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Application.Common.Models.Templates;
using F4ConversationCloud.Domain.Enum;
using F4ConversationCloud.Infrastructure.Persistence;
using F4ConversationCloud.SuperAdmin.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.RegularExpressions;
using static F4ConversationCloud.SuperAdmin.Models.TemplateViewModel;

namespace F4ConversationCloud.SuperAdmin.Controllers
{
    public class TemplateManagementController : Controller
    {
        private readonly ITemplateManagementService _templateManagementService;
        private readonly ITemplateRepositories _templateRepositories;
        private readonly IWhatsAppTemplateService _whatsAppTemplateService;
        private readonly DbContext _context;

        public TemplateManagementController(ITemplateManagementService templateManagementService, ITemplateRepositories templateRepositories,
            IWhatsAppTemplateService whatsAppTemplateService, DbContext context)
        {
            _templateManagementService = templateManagementService;
            _templateRepositories = templateRepositories;
            _whatsAppTemplateService = whatsAppTemplateService;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        //[HttpGet("create-template")]
        //public IActionResult CreateTemplate()
        //{
        //    return View();
        //}

        //[HttpPost("create-template")]
        //public IActionResult CreateTemplate(CreateTemplateViewModel Request)
        //{
        //    try
        //    {
        //        if(!ModelState.IsValid)
        //        {
        //            return View(Request);
        //        }

        //        var templateRequest = new WhatsAppTemplateRequest
        //        {
        //            Name = Request.TemplateName,
        //            Language = Request.Language,
        //            Category = Request.Category,
        //            Components = TemplateComponentsRequestHandler.ComponetRequest(Request).Result,
        //        };


        //        var jsoneserialiazer = JsonSerializer.Serialize(templateRequest);

        //        var createTemplate = _templateManagementService.CreateTemplate(templateRequest);

        //        return View(Request);
        //    }
        //    catch (Exception)
        //    {

        //        return View(Request);
        //    }


        //}

        [HttpGet("update-template/{Template_id}")]
        public async Task<IActionResult> UpdateTemplate(string Template_id)
        {
            try
            {
                var template = await _templateManagementService.GetTemplateByIdAsync(Template_id);
                return View(template);
            }
            catch (Exception)
            {
                return View();
            }
        }
        //[HttpPost]
        //public async Task<IActionResult> DeleteTemplate(string Template_id, int ClientInfoId, string TemplateName)
        //{

        //    try
        //    {
        //        dynamic isDeletedResponce = await _templateManagementService.DeleteTemplateById(Template_id, ClientInfoId, TemplateName);
        //        if (isDeletedResponce != null)
        //        {
        //            TempData["SuccessMessage"] = isDeletedResponce.message;
        //            return Json(isDeletedResponce);
        //        }
        //        else
        //        {
        //            TempData["SuccessMessage"] = "Unknown error occurred.";
        //            return Json(new DeleteTemplateResponse { success = false, message = "Unknown error occurred." });
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        return Json(new DeleteTemplateResponse { success = false, message = ex.Message });
        //    }
        //}

        public async Task<IActionResult> List(TemplatesListViewModel model)
        {
            try
            {
                model.StatusList = EnumExtensions.ToSelectList<TemplateApprovalStatus>();
                model.LanguageList = EnumExtensions.ToSelectList<TemplateLanguages>();
                model.TemplateCategoryList = EnumExtensions.ToSelectList<TemplateModuleType>();

                var response = await _whatsAppTemplateService.GetFilteredTemplatesByWABAId(new TemplateListFilter
                {
                    TemplateNameFilter = model.TemplateNameFilter ?? String.Empty,
                    TemplateCategoryFilter = model.TemplateCategoryFilter,
                    LanguageFilter = model.LanguageFilter,
                    CreatedOnFilter = model.CreatedOnFilter ?? String.Empty,
                    StatusFilter = model.StatusFilter,
                    PageNumber = model.PageNumber,
                    PageSize = model.PageSize,
                });

                if (model.PageNumber > 1 && Math.Ceiling((decimal)response.Item2 / (decimal)model.PageSize) < model.PageNumber)
                {
                    if (model.PageNumber > 1)
                    {
                        TempData["ErrorMessage"] = "Invalid Page";
                    }
                    return RedirectToAction("List");
                }

                model.TotalCount = response.Item2;
                model.data = response.Item1.ToList().Select(x => new TemplatesListViewModel.TemplateListViewItem()
                {
                    Id = x.Id,
                    SrNo = x.SrNo,
                    TemplateName = x.TemplateName,
                    TemplateCategory = x.TemplateCategory,
                    CreatedOn = x.CreatedOn,
                    Status = x.Status
                });

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong. Please contact your administrator.";
                return View(new TemplatesListViewModel());
            }
        }

        public async Task<IActionResult> CreateTemplate()
        {
            try
            {
                var viewModel = new TemplateViewModel();
                viewModel.TemplateCategoryList = EnumExtensions.ToSelectList<TemplateModuleType>();
                viewModel.LanguageList = EnumExtensions.ToSelectList<TemplateLanguages>();
                viewModel.VariableTypeList = EnumExtensions.ToSelectList<VariableTypes>();
                viewModel.MediaTypeList = EnumExtensions.ToSelectList<MediaType>();
                viewModel.MarketingTemplateTypeList = EnumExtensions.ToSelectList<MarketingTemplateType>();
                viewModel.UtilityTemplateTypeList = EnumExtensions.ToSelectList<UtilityTemplateType>();
                viewModel.AuthenticationTemplateTypeList = EnumExtensions.ToSelectList<AuthenticationTemplateType>();

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong. Please contact your administrator.";
                return View(new TemplateViewModel());
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTemplate(TemplateViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    model.TemplateCategoryList = EnumExtensions.ToSelectList<TemplateModuleType>();
                    model.LanguageList = EnumExtensions.ToSelectList<TemplateLanguages>();
                    model.VariableTypeList = EnumExtensions.ToSelectList<VariableTypes>();
                    model.MediaTypeList = EnumExtensions.ToSelectList<MediaType>();
                    model.MarketingTemplateTypeList = EnumExtensions.ToSelectList<MarketingTemplateType>();
                    model.UtilityTemplateTypeList = EnumExtensions.ToSelectList<UtilityTemplateType>();
                    model.AuthenticationTemplateTypeList = EnumExtensions.ToSelectList<AuthenticationTemplateType>();

                    return View(model);
                }

                var request = new TemplateViewRequestModel();
                request.TemplateName = model.TemplateName;
                request.TemplateCategory = model.TemplateCategory;
                request.TemplateCategoryName = model.TemplateCategoryName;
                request.TemplateType = model.TemplateType;
                request.TemplateTypeName = model.TemplateTypeName;
                request.Language = model.Language;
                request.VariableType = model.VariableType;
                request.MediaType = model.MediaType;
                request.File = model.File;
                request.FileName = model.FileName;
                request.FileUrl = model.FileUrl;
                request.Header = model.Header;
                request.MessageBody = model.MessageBody;
                request.Footer = model.Footer;
                request.HeaderVariableName = model.HeaderVariableName;
                request.HeaderVariableValue = model.HeaderVariableValue;
                request.bodyVariables = model.bodyVariables
                .Select(v => new TemplateViewRequestModel.BodyVariable
                {
                    BodyVariableName = v.BodyVariableName,
                    BodyVariableValue = v.BodyVariableValue
                }).ToList();
                request.ClientInfoId = model.ClientInfoId;
                request.MetaConfigId = model.MetaConfigId;
                request.WABAId = model.WABAId;
                request.PageMode = model.PageMode;
                request.TemplateId = model.TemplateId;

                APIResponse result = new APIResponse();
                if (request.PageMode == "Edit")
                {
                    result = await _templateRepositories.BuildAndEditTemplate(request);
                }
                else
                {
                    result = await _templateRepositories.BuildAndCreateTemplate(request);
                }

                if (result.Status)
                {
                    TempData["SuccessMessage"] = result.Message;
                }
                else
                {
                    TempData["ErrorMessage"] = result.Message;
                }
                return RedirectToAction("ClientDetails", "ClientManagement", new { Id = model.MetaConfigId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong. Please contact your administrator.";
                return View(new TemplateViewModel());
            }
        }

        public async Task<IActionResult> UpdateTemplate([FromRoute] int id)
        {
            try
            {
                var data = await _whatsAppTemplateService.GetTemplateByIdAsync(id);

                var viewModel = new TemplateViewModel();
                viewModel.TemplateCategoryList = EnumExtensions.ToSelectList<TemplateModuleType>();
                viewModel.LanguageList = EnumExtensions.ToSelectList<TemplateLanguages>();
                viewModel.VariableTypeList = EnumExtensions.ToSelectList<VariableTypes>();
                viewModel.MediaTypeList = EnumExtensions.ToSelectList<MediaType>();
                viewModel.MarketingTemplateTypeList = EnumExtensions.ToSelectList<MarketingTemplateType>();
                viewModel.UtilityTemplateTypeList = EnumExtensions.ToSelectList<UtilityTemplateType>();
                viewModel.AuthenticationTemplateTypeList = EnumExtensions.ToSelectList<AuthenticationTemplateType>();
                viewModel.TemplateType = Convert.ToInt32(data.Category);
                viewModel.TemplateTypeName = EnumExtensions.GetDisplayNameById<TemplateModuleType>(Convert.ToInt32(data.Category));
                viewModel.TemplateCategory = 1;
                viewModel.TemplateCategoryName = EnumExtensions.GetDisplayNameById<UtilityTemplateType>(1);
                viewModel.TemplateName = data.TemplateName;
                viewModel.VariableType = 1;
                viewModel.MediaType = 0;
                viewModel.Header = data.RawHeader;
                viewModel.HeaderVariableValue = data.HeaderExample;
                viewModel.HeaderVariableName = "{{1}}";
                viewModel.MessageBody = data.RawBody;
                viewModel.Footer = data.FooterText;
                viewModel.TemplateId = data.TemplateId;

                viewModel.bodyVariables = new List<BodyVariable>();
                if (!string.IsNullOrEmpty(data.BodyExample))
                {
                    var examples = data.BodyExample.Split(',');

                    for (int i = 0; i < examples.Length; i++)
                    {
                        var bodyVariable = new BodyVariable
                        {
                            BodyVariableName = examples[i],
                            BodyVariableValue = $"{{{{{i + 1}}}}}"
                        };

                        viewModel.bodyVariables.Add(bodyVariable);
                    }
                }
                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong. Please contact your administrator.";
                return View(new TemplateViewModel());
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTemplate(int TemplateId)
        {
            try
            {
                var isDeletedResponce = await _whatsAppTemplateService.DeactivateTemplateAsync(TemplateId);

                if (isDeletedResponce.success)
                {
                    return Json(new DeleteTemplateResponse { success = true, message = isDeletedResponce.message });
                }
                else
                {
                    return Json(new DeleteTemplateResponse { success = false, message = isDeletedResponce.message });
                }
            }
            catch (Exception ex)
            {
                return Json(new DeleteTemplateResponse { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> ActivateTemplate(int TemplateId)
        {
            try
            {
                var result = await _whatsAppTemplateService.ActivateTemplateAsync(TemplateId);
                if (result.success)
                    return Json(new { success = true, message = result.message });
                else
                    return Json(new { success = false, message = result.message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }



        [HttpPost]
        public async Task<IActionResult> GetTemplateTypePartialView(TemplateViewModel model)
        {
            var viewName = "";

            if (model != null)
            {
                var templateCategoryName = model.TemplateCategoryName;
                if (templateCategoryName == TemplateModuleType.Marketing.Get<DisplayAttribute>().Name)
                {
                    viewName = "_MarketingTemplateTypeOptions";
                    model.MarketingTemplateTypeList = EnumExtensions.ToSelectList<MarketingTemplateType>();
                }
                else if (templateCategoryName == TemplateModuleType.Utility.Get<DisplayAttribute>().Name)
                {
                    viewName = "_UtilityTemplateTypeOptions";
                    model.UtilityTemplateTypeList = EnumExtensions.ToSelectList<UtilityTemplateType>();
                }
                else if (templateCategoryName == TemplateModuleType.Authentication.Get<DisplayAttribute>().Name)
                {
                    viewName = "_AuthenticationTemplateTypeOptions";
                    model.AuthenticationTemplateTypeList = EnumExtensions.ToSelectList<AuthenticationTemplateType>();
                }
            }

            return PartialView(viewName, model);
        }

        [HttpPost]
        public async Task<IActionResult> GetTemplateFormPartialView(TemplateViewModel model)
        {
            var viewName = "";

            if (model != null)
            {
                var templateCategoryName = EnumExtensions.GetDisplayNameById<TemplateModuleType>(model.TemplateCategory);
                model.TemplateCategoryName = templateCategoryName;

                if (templateCategoryName == TemplateModuleType.Marketing.Get<DisplayAttribute>().Name)
                {
                    if (model.TemplateType == (int)MarketingTemplateType.Default)
                    {
                        viewName = "_MarketingDefaultTemplate";
                    }
                    else if (model.TemplateType == (int)MarketingTemplateType.Catalogue)
                    {

                    }
                    else if (model.TemplateType == (int)MarketingTemplateType.CallingPermissionsRequest)
                    {

                    }
                    else if (model.TemplateType == (int)MarketingTemplateType.Carousel)
                    {
                        viewName = "_MarketingCarouselTemplate";
                    }
                }
                else if (templateCategoryName == TemplateModuleType.Utility.Get<DisplayAttribute>().Name)
                {
                    if (model.TemplateType == (int)UtilityTemplateType.Default)
                    {
                        viewName = "_UtilityDefaultTemplate";
                    }
                    else if (model.TemplateType == (int)UtilityTemplateType.CallingPermissionsRequest)
                    {

                    }
                    else if (model.TemplateType == (int)UtilityTemplateType.Carousel)
                    {
                        viewName = "_UtilityCarouselTemplate";
                    }
                }
                else if (templateCategoryName == TemplateModuleType.Authentication.Get<DisplayAttribute>().Name
                && model.TemplateType == (int)AuthenticationTemplateType.OneTimePasscode)
                {
                    viewName = "_AuthenticationOneTimeCodeTemplate";
                }

                model.TemplateCategoryList = EnumExtensions.ToSelectList<TemplateModuleType>();
                model.LanguageList = EnumExtensions.ToSelectList<TemplateLanguages>();
                model.VariableTypeList = EnumExtensions.ToSelectList<VariableTypes>();
                model.MediaTypeList = EnumExtensions.ToSelectList<MediaType>();
                model.MarketingTemplateTypeList = EnumExtensions.ToSelectList<MarketingTemplateType>();
                model.UtilityTemplateTypeList = EnumExtensions.ToSelectList<UtilityTemplateType>();
                model.AuthenticationTemplateTypeList = EnumExtensions.ToSelectList<AuthenticationTemplateType>();
                ModelState.Clear();
            }
            ViewData["Index"] = 0;
            return PartialView(viewName, model);
        }

        [HttpPost]
        public IActionResult RenderBodyVariablesPartial([FromBody] List<string> variableNumbers)
        {
            var model = new TemplateViewModel
            {
                bodyVariables = variableNumbers.Select(v => new BodyVariable
                {
                    BodyVariableValue = $"{{{{{v}}}}}",
                    BodyVariableName = ""
                }).ToList()
            };

            return PartialView("_MessageVariableBody", model);
        }
    }
}