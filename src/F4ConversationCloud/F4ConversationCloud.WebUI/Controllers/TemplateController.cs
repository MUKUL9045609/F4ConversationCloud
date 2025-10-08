using F4ConversationCloud.Application.Common.Interfaces.Services;
using F4ConversationCloud.Application.Common.Models.Templates;
using F4ConversationCloud.Domain.Entities;
using F4ConversationCloud.Infrastructure.Service;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Twilio.TwiML.Voice;

namespace F4ConversationCloud.WebUI.Controllers
{
    [ApiController]
    [Route("Template")]
    public class TemplateController : Controller
    {
        private readonly ITemplateService _templateService;

        public TemplateController(ITemplateService templateService)
        {

            _templateService = templateService;
        }


        [HttpPost("CreateTemplate")]
        public async Task<IActionResult> CreateTemplate(MessageTemplate request)
        {
            try
            {
                List<dynamic> components = new List<dynamic>();

                HeaderComponent headerComponent = JsonSerializer.Deserialize<HeaderComponent>(request.components[0].GetRawText());
                BodyComponent bodyComponent = JsonSerializer.Deserialize<BodyComponent>(request.components[1].GetRawText());
                FooterComponent footerComponent = JsonSerializer.Deserialize<FooterComponent>(request.components[2].GetRawText());
                ButtonComponent buttonComponent = JsonSerializer.Deserialize<ButtonComponent>(request.components[3].GetRawText());

                MessageTemplate messageTemplate = new MessageTemplate();
                messageTemplate.category = request.category;
                messageTemplate.name = request.name;
                messageTemplate.language = request.language;

                components.Add(headerComponent);
                components.Add(bodyComponent);
                components.Add(footerComponent);
                components.Add(buttonComponent);
                messageTemplate.components = new List<dynamic>();
                messageTemplate.components.AddRange(components);

                await _templateService.CreateTemplate(messageTemplate);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500);

            }
        }



    }
}
