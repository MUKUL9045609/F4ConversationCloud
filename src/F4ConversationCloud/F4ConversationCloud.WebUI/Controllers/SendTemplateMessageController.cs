using F4ConversationCloud.Application.Common.Interfaces.Repositories;
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
        private readonly ISendTemplateMessageRepository _messageRepository;

        public SendTemplateMessagesController(ISendTemplateMessageService messageService, ISendTemplateMessageRepository messageRepository)
        {

            _messageService = messageService;
            _messageRepository = messageRepository;
        }

        [HttpPost("Message")]
        public IActionResult SendMessage(SendTemplateDTO sendTemplateDTO)
        {
            try
            {
                var result = _messageService.SendTemplateMessageAsync(sendTemplateDTO);
                if (result != null)
                {
                    //string PhoneNumberId, int TemplateId, int ConversationType,string MessageSentFrom,string MessageSentTo, string MessageSentStatus
                    var details = _messageRepository.InsertIntoTemplateInsights(sendTemplateDTO.PhoneId,1,1,"","","") ;
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }



    }
}
