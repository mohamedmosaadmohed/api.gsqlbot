using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GSQLBOT.Presentation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatMessageController : ControllerBase
    {
        [HttpGet("ki")]
        public async Task<IActionResult> GetChatMessages(int chatId,int UserId)
        {
            return Ok();
        }
        [HttpPost("ou")]
        public async Task<IActionResult> AddChatMessages(int chatId, int UserId)
        {
            return Ok();
        }
        [HttpGet("rtw")]
        public async Task<IActionResult> UpdateChatMessage(int chatMessageId,int chatId, int UserId)
        {
            return Ok();
        }
    }
}
