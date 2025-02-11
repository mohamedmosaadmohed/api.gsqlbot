using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GSQLBOT.Presentation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        [HttpPost("new-chat")]
        public async Task<IActionResult> NewChat()
        {
            return Ok();
        }
        [HttpGet("all-chats")]
        public async Task<IActionResult> GetAllChats(int UserId)
        {
            return Ok();
        }
        [HttpPost("fr")]
        public async Task<IActionResult> GetChatbyId(int Id, int UserId)
        {
            return Ok();
        }
        [HttpDelete("we")]
        public async Task<IActionResult> DeleteChatbyId(int Id, int UserId)
        {
            return Ok();
        }
        [HttpDelete("cdsa")]
        public async Task<IActionResult> DeleteAllChatUser(int UserId)
        {
            return Ok();
        }
        [HttpPost("search")]
        public async Task<IActionResult> Search(string quary, int UserId)
        {
            return Ok();
        }
    }
}
