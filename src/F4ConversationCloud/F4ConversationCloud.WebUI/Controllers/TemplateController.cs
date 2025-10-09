using F4ConversationCloud.Application.Common.Interfaces.Services;
using F4ConversationCloud.Application.Common.Models.Templates;
using F4ConversationCloud.Domain.Entities;
using F4ConversationCloud.Infrastructure.Service;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Nodes;
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

                MessageTemplate messageTemplate = new MessageTemplate();
                messageTemplate.category = request.category;
                messageTemplate.name = request.name;
                messageTemplate.language = request.language;

                foreach (var component in request.components)
                {

                    if (component.TryGetProperty("type", out JsonElement typeElement))
                    {
                        string typeValue = typeElement.GetString().ToLower();
                        if (typeValue == "header")
                        {
                            HeaderComponent headerComponent = JsonSerializer.Deserialize<HeaderComponent>(request.components[0].GetRawText());
                            components.Add(headerComponent);
                        }
                        else if (typeValue == "body")
                        {
                            BodyComponent bodyComponent = JsonSerializer.Deserialize<BodyComponent>(request.components[1].GetRawText());
                            components.Add(bodyComponent);
                        }
                        else if (typeValue == "footer")
                        {
                            FooterComponent footerComponent = JsonSerializer.Deserialize<FooterComponent>(request.components[2].GetRawText());
                            components.Add(footerComponent);
                        }
                        else if (typeValue == "buttons")
                        {
                            ButtonComponent buttonComponent = JsonSerializer.Deserialize<ButtonComponent>(request.components[3].GetRawText());
                            components.Add(buttonComponent);
                        }
                    }
                }

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

        [HttpPost("EditTemplate")]
        public async Task<IActionResult> EditTemplate(MessageTemplate request)
        {
            try
            {
                List<dynamic> components = new List<dynamic>();

                MessageTemplate messageTemplate = new MessageTemplate();
                messageTemplate.category = request.category;
                messageTemplate.name = request.name;
                messageTemplate.language = request.language;

                foreach (var component in request.components)
                {

                    if (component.TryGetProperty("type", out JsonElement typeElement))
                    {
                        string typeValue = typeElement.GetString().ToLower();
                        if (typeValue == "header")
                        {
                            HeaderComponent headerComponent = JsonSerializer.Deserialize<HeaderComponent>(request.components[0].GetRawText());
                            components.Add(headerComponent);
                        }
                        else if (typeValue == "body")
                        {
                            BodyComponent bodyComponent = JsonSerializer.Deserialize<BodyComponent>(request.components[1].GetRawText());
                            components.Add(bodyComponent);
                        }
                        else if (typeValue == "footer")
                        {
                            FooterComponent footerComponent = JsonSerializer.Deserialize<FooterComponent>(request.components[2].GetRawText());
                            components.Add(footerComponent);
                        }
                        else if (typeValue == "buttons")
                        {
                            ButtonComponent buttonComponent = JsonSerializer.Deserialize<ButtonComponent>(request.components[3].GetRawText());
                            components.Add(buttonComponent);
                        }
                    }
                }

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


        [HttpPost("DeleteTemplate")]
        public async Task<IActionResult> DeleteTemplate(int Template_Id , string Template_Name)
        {
            try
            {
                await _templateService.DeleteTemplate(Template_Id, Template_Name);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500);

            }
        }

        [HttpPost("DeleteTemplateByName")]
        public async Task<IActionResult> DeleteTemplateByName(string Template_Name)
        {
            try
            {
                await _templateService.DeleteTemplateByName(Template_Name);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500);

            }
        }


    }
}
