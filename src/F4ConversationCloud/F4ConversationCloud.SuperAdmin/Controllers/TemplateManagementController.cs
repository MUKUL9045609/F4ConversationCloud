using F4ConversationCloud.Application.Common.Interfaces.Services;
using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models.MetaCloudApiModel.Templates;
using F4ConversationCloud.Application.Common.Models.SuperAdmin;
using F4ConversationCloud.Application.Common.Models.Templates;
using F4ConversationCloud.Domain.Entities;
using F4ConversationCloud.SuperAdmin.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel;
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


              

                var templateRequest = new MessageTemplate
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
                return PartialView("_TemplateList",templates);
            }
            catch (Exception)
            {

               return PartialView("_TemplateList", new List<TemplatesListViewModel>());
            }

        }



    }
}
