using F4ConversationCloud.Application.Common.Interfaces.Services;
using F4ConversationCloud.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace F4ConversationCloud.WebUI.Controllers
{
    [ApiController]
    [Route("webhook")]
    public class WebhookController : Controller
    {
        private readonly IWebhookService _webhookService;

        public WebhookController(IWebhookService webhookService)
        {
            _webhookService = webhookService;
        }


        [HttpGet("receive")]
        public IActionResult VerifyWebhook([FromQuery(Name = "hub.mode")] string mode, [FromQuery(Name = "hub.verify_token")] string token, [FromQuery(Name = "hub.challenge")] string challenge)
        {
            if (mode == "subscribe" && token == "b29f91a0-94cd-4f82-b4e0-60f6d2ef1b52")
            {
                return Ok(challenge);
            }
            return Unauthorized();
        }

        [HttpPost("receive")]
        public async Task<IActionResult> ReceiveMessage([FromBody] JsonElement root)
        {
            try
            {
                var payload = JsonSerializer.Deserialize<WhatsAppWebhookPayload>(root.GetRawText());

                if (payload?.Entry?.FirstOrDefault()?.Changes?.FirstOrDefault()?.Value?.Messages?.FirstOrDefault() == null)
                {
                    return BadRequest("Invalid message payload.");
                }

                await _webhookService.CallWebhookAsync(payload);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500);

            }
        }
    }
}
