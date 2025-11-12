using F4ConversationCloud.Application.Common.Handler;
using F4ConversationCloud.Application.Common.Interfaces.Repositories;
using F4ConversationCloud.Application.Common.Interfaces.Services;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.Templates;
using F4ConversationCloud.Domain.Entities;
using F4ConversationCloud.Infrastructure.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Nodes;
using Twilio.TwiML.Voice;

namespace F4ConversationCloud.WebUI.Controllers
{
    [Authorize(Roles = "1")]
    [ApiController]
    [Route("MessageTemplates")]
    public class TemplateController : Controller
    {
        private readonly ITemplateService _templateService;
        private readonly ITemplateRepositories _templateRepositories;

        public TemplateController(ITemplateService templateService, ITemplateRepositories templateRepositories)
        {

            _templateService = templateService;
            _templateRepositories = templateRepositories;
        }

        [HttpPost("[action]")]
        [Consumes("application/json")]
        [HasPermission("IsCreateTemplate")]
        public async Task<IActionResult> CreateTemplate([FromBody] TemplateRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(new { Errors = errors });
            }

            try
            {
                var result = await _templateRepositories.MetaCreateTemplate(request);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("[action]")]
        [HasPermission("IsEditTemplate")]
        public async Task<IActionResult> EditTemplate(EditTemplateRequest request)
        {

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            }

            try
            {
                
                var result = await _templateRepositories.MetaEditTemplate(request);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500);

            }
        }


        [HttpPost("[action]")]
        [HasPermission("IsDeleteTemplate")]
        public async Task<IActionResult> DeleteTemplate(int Template_Id, string Template_Name)
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

        [HttpPost("[action]")]
        [HasPermission("IsDeleteTemplate")]
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

        [HttpPost("[action]")]
        [HasPermission("IsView")]
        public async Task<IActionResult> ViewTemplate(int Template_Id, string Template_Name)
        {
            try
            {


                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500);

            }
        }

        [HttpPost("[action]")]
        [HasPermission("IsView")]
        public async Task<IActionResult> TemplateList(int Template_Id, string Template_Name)
        {
            try
            {


                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500);

            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UploadFacebookImage([FromBody] UploadImage uploadImage)
        {
            try
            {
                var response = await _templateService.UploadMetaImage(uploadImage.base64Image);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500);

            }
        }

    }
}
