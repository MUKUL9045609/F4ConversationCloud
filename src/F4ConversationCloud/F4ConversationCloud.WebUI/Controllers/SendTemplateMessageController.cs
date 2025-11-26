using F4ConversationCloud.Application.Common.Interfaces.Services;
using F4ConversationCloud.Application.Common.Models.Templates;
using Microsoft.AspNetCore.Mvc;

namespace F4ConversationCloud.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendTemplateMessagesController : ControllerBase
    {
        private readonly ISendTemplateMessageService _messageService;
        
        public SendTemplateMessagesController(ISendTemplateMessageService messageService) { 
        
            _messageService = messageService;
        }

        [HttpPost("Message")]
        public IActionResult SendMessage(SendTemplateDTO sendTemplateDTO)
        {
            try
            {
                var result = _messageService.SendTemplateMessageAsync(sendTemplateDTO);

                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }



    }
}
