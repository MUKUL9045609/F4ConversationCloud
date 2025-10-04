using F4ConversationCloud.Application.Common.Interfaces.Services.SuperAdmin;
using F4ConversationCloud.Application.Common.Models.MetaCloudApiModel.Templates;
using F4ConversationCloud.Domain.Entities;
using F4ConversationCloud.SuperAdmin.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;

namespace F4ConversationCloud.SuperAdmin.Controllers
{
    public class TemplateManagementController : Controller
    {
        private readonly ITemplateManagementService _templateManagementService;
        public TemplateManagementController(ITemplateManagementService templateManagementService)
        {
            _templateManagementService = templateManagementService;
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
                    Name = Request.TemplateName,
                    Language = Request.Language,
                    Category = Request.Category,
                    Components = components
                };
                var jsoneserialiazer = JsonSerializer.Serialize(templateRequest);
                var createTemplate = _templateManagementService.CreateTemplate(templateRequest);

                return View(Request);
            }
            catch (Exception)
            {

               return View(Request);
            }
          
            
        }
    }
}
