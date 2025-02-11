using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GSQLBOT.Presentation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserConfigrationController : ControllerBase
    {
        [HttpGet("bxncxn")]
        public async Task<IActionResult> GetAllUserConfig(int UserId)
        {
            return Ok();
        }
        [HttpGet("rtsh")]
        public async Task<IActionResult> GetUserConfigbyId(int UserId,int UserConfigId)
        {
            return Ok();
        }
        [HttpPost("ahethwre")]
        public async Task<IActionResult> AddUserConfig()
        {
            return Ok();
        }
        [HttpPut("qqqqqq")]
        public async Task<IActionResult> UpdateUserConfig()
        {
            return Ok();
        }
        [HttpDelete("uuuuuuuuuuuuuu")]
        public async Task<IActionResult> DeleteUserConfig(int UserId, int UserConfigId)
        {
            return Ok();
        }
    }
}
