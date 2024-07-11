using DesigneryCore.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DesingeryWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet("get-all-messages")]
        public async Task<IActionResult> GetAllMessages()
        {
            var messages = await _messageService.GetAllMessagesAsync();
            return Ok(messages);
        }

        [HttpGet("get-message/{messageId}")]
        public async Task<IActionResult> GetMessage(int messageId)
        {
            var message = await _messageService.GetMessageByIdAsync(messageId);
            if (message != null)
            {
                return Ok(message);
            }
            else
            {
                return NotFound("Message not found.");
            }
        }

        [HttpDelete("delete-message/{messageId}")]
        public async Task<IActionResult> DeleteMessage(int messageId)
        {
            bool isDeleted = await _messageService.DeleteMessageAsync(messageId);
            if (isDeleted)
            {
                return Ok("Message deleted successfully.");
            }
            else
            {
                return NotFound("Message not found.");
            }

        }
    }
}
