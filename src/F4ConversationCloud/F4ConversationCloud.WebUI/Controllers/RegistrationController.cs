using F4ConversationCloud.Application.Common.Handler;
using F4ConversationCloud.Application.Common.Interfaces.Services.Meta;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.Templates;
using Microsoft.AspNetCore.Mvc;

namespace F4ConversationCloud.WebUI.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly IMetaService _metaService;
        public RegistrationController(IMetaService metaService)
        {
            _metaService = metaService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RegisterPhoneNumber([FromBody] PhoneRegistrationOnMeta request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(new { Errors = errors });
            }

            try
            {
                await _metaService.RegisterPhone(request);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> DeregisterPhoneNumber([FromBody] PhoneRegistrationOnMeta request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(new { Errors = errors });
            }

            try
            {
                await _metaService.DeregisterPhone(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


    }
}
