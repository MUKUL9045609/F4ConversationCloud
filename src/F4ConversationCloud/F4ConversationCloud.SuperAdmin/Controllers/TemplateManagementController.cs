using BuldanaUrban.Domain.Helpers;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models.MetaCloudApiModel.Templates;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Domain.Enum;
using F4ConversationCloud.SuperAdmin.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json;

namespace F4ConversationCloud.SuperAdmin.Controllers
{
    public class TemplateManagementController : Controller
    {
        private readonly ITemplateManagementService _templateManagementService;
        private readonly ITemplateService _templateService;
        public TemplateManagementController(ITemplateManagementService templateManagementService , ITemplateService templateService)
        {
            _templateManagementService = templateManagementService;
            _templateService = templateService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("create-template")]
        public IActionResult CreateTemplate()
        {
            return View();
        }

        [HttpPost("create-template")]
        public IActionResult CreateTemplate(CreateTemplateViewModel Request)
        {
            try
            {
                var components = (Request.Components ?? new List<ComponentViewModel>())
                                       .Where(c => !string.IsNullOrWhiteSpace(c.Text)
                                           || (c.Buttons?.Any(b => !string.IsNullOrWhiteSpace(b.Text)) == true)

                                           )
                                       .Select(c => new CreateTemplateComponent
                                       {
                                           Type = c.Type,
                                           Text = c.Text,
                                           Format = c.Format,
                                           Buttons = c.Buttons?
                                                   .Where(b => !string.IsNullOrWhiteSpace(b.Text))
                                                   .Select(b => new TemplateButton
                                                   {
                                                       Text = b.Text,
                                                       Type = b.Type,
                                                       Url = b.Url
                                                   }).ToList(),
                                           Example = c.Example != null
                                                    ? new Example
                                                    {
                                                        HeaderText = (c.Example.HeaderText?.Any() == true)
                                                            ? c.Example.HeaderText
                                                            : new List<string>(),

                                                        BodyText = c.Example.BodyText?.ToList()
                                                            ?? new List<List<string>>()

                                                    }
                                                 : null
                                       }).ToList();

                var templateRequest = new WhatsAppTemplateRequest
                {
                    name = Request.TemplateName,
                    language = Request.Language,
                    category = Request.Category
                };

                templateRequest.components = new List<dynamic>();
                templateRequest.components.AddRange(components);

                var jsoneserialiazer = JsonSerializer.Serialize(templateRequest);

                var createTemplate = _templateService.CreateTemplate(templateRequest);

                return View(Request);
            }
            catch (Exception)
            {

                return View(Request);
            }


        }


        [HttpGet("List")]
        public async Task<IActionResult> List(TemplatesListViewModel model)
        {
            try
            {
                var filter = new TemplatesListFilter
                {
                    PageNumber = model.PageNumber,
                    PageSize = model.PageSize
                };

                var templates = await _templateManagementService.TemplateListAsync(filter);
                return PartialView("_TemplateList", templates);
            }
            catch (Exception)
            {

                return PartialView("_TemplateList", new List<TemplatesListViewModel>());
            }

        }

        public async Task<IActionResult> Create()
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
                return StatusCode(500, false);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(TemplateViewModel model)
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

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong. Please contact your administrator.";
                return StatusCode(500, false);
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
                var templateCategoryName = model.TemplateCategoryName;
                var templateTypeName = model.TemplateTypeName;

                if (templateCategoryName == TemplateModuleType.Marketing.Get<DisplayAttribute>().Name)
                {
                    if (templateTypeName == MarketingTemplateType.Default.Get<DisplayAttribute>().Name)
                    {
                        viewName = "_MarketingDefaultTemplate";
                    }
                    else if (templateTypeName == MarketingTemplateType.Catalogue.Get<DisplayAttribute>().Name)
                    {

                    }
                    else if (templateTypeName == MarketingTemplateType.CallingPermissionsRequest.Get<DisplayAttribute>().Name)
                    {

                    }
                }
                else if (templateCategoryName == TemplateModuleType.Utility.Get<DisplayAttribute>().Name)
                {
                    if (templateTypeName == UtilityTemplateType.Default.Get<DisplayAttribute>().Name)
                    {
                        viewName = "_UtilityDefaultTemplate";
                    }
                    else if (templateTypeName == UtilityTemplateType.CallingPermissionsRequest.Get<DisplayAttribute>().Name)
                    {

                    }
                }
                else if (templateCategoryName == TemplateModuleType.Authentication.Get<DisplayAttribute>().Name
                && templateTypeName == AuthenticationTemplateType.OneTimePasscode.Get<DisplayAttribute>().Name)
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

            return PartialView(viewName, model);
        }
    }
}