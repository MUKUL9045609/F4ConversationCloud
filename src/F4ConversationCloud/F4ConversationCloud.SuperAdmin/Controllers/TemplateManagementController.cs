using BuldanaUrban.Domain.Helpers;
using F4ConversationCloud.Application.Common.Interfaces.Repositories;
using F4ConversationCloud.Application.Common.Interfaces.Services.Common;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models.CommonModels;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Application.Common.Models.Templates;
using F4ConversationCloud.Domain.Enum;
using F4ConversationCloud.SuperAdmin.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace F4ConversationCloud.SuperAdmin.Controllers
{
    public class TemplateManagementController : Controller
    {
        private readonly ITemplateManagementService _templateManagementService;
        private readonly ITemplateRepositories _templateRepositories;
        private readonly IWhatsAppTemplateService _whatsAppTemplateService;

        public TemplateManagementController(ITemplateManagementService templateManagementService, ITemplateRepositories templateRepositories)
        {
            _templateManagementService = templateManagementService;
            _templateRepositories = templateRepositories;
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
        [HttpPost]
        public async Task<IActionResult> DeleteTemplate(string Template_id, int ClientInfoId, string TemplateName)
        {

            try
            {
                dynamic isDeletedResponce = await _templateManagementService.DeleteTemplateById(Template_id, ClientInfoId, TemplateName);
                if (isDeletedResponce != null)
                {
                    TempData["SuccessMessage"] = isDeletedResponce.message;
                    return Json(isDeletedResponce);
                }
                else
                {
                    TempData["SuccessMessage"] = "Unknown error occurred.";
                    return Json(new DeleteTemplateResponse { success = false, message = "Unknown error occurred." });
                }

            }
            catch (Exception ex)
            {

                return Json(new DeleteTemplateResponse { success = false, message = ex.Message });
            }
        }

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
                var templateRequest = new TemplateRequest();

                templateRequest.Name = model.TemplateName;
                //templateRequest.Language = EnumExtensions.GetDisplayNameById<TemplateLanguages>(model.Language);
                templateRequest.Language = "en";
                //templateRequest.Category = EnumExtensions.GetDisplayNameById<TemplateModuleType>(model.TemplateCategory);
                templateRequest.Category = "UTILITY";
                templateRequest.TemplateHeader.Type = "HEADER";
                templateRequest.TemplateHeader.Format = "TEXT";
                templateRequest.TemplateHeader.Text = model.Header;
                templateRequest.TemplateHeader.Example = new HeaderExample
                {
                    Header_Text = new List<string> { model.HeaderVariableValue },
                    Format = "TEXT"
                };
                templateRequest.TemplateBody.Type = "BODY";
                templateRequest.TemplateBody.Text = model.MessageBody;
                //templateRequest.TemplateBody.Text = "Shop now through {{1}} and use code {{2}} to get {{3}} off of all merchandise.\r\n";
                templateRequest.TemplateBody.Body_Example = new Application.Common.Models.Templates.BodyExample
                {
                    Body_Text = new List<List<string>>
                        {
                            new List<string> { "the end of August", "25OFF", "25%" }
                        }
                };
                templateRequest.TemplateFooter.type = "FOOTER";
                templateRequest.TemplateFooter.text = model.Footer;
                templateRequest.ClientInfoId = model.ClientInfoId.ToString();
                templateRequest.WABAID = model.WABAId;

                await _templateRepositories.MetaCreateTemplate(templateRequest);

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

        //public IActionResult AddMessageVariable(int index)
        //{
        //    var model = new TemplateViewModel();

        //    // Ensure the list is initialized
        //    if (model.bodyVariables == null)
        //        model.bodyVariables = new List<BodyVariable>();

        //    // Add empty items until the list has the required index
        //    while (model.bodyVariables.Count <= index)
        //    {
        //        model.bodyVariables.Add(new BodyVariable());
        //    }

        //    ViewData["Index"] = index;
        //    return PartialView("_MessageVariableBody", model);
        //}
    }
}