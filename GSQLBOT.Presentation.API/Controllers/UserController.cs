using GSQLBOT.Core.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GSQLBOT.Presentation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok();
        }
        [HttpPost("yio")]
        public async Task<IActionResult> GetbyId(int Id)
        {
            return Ok();
        }
        [HttpPost("wrty")]
        public async Task<IActionResult> AddUser()
        {
            return Ok();
        }
        [HttpPost("update-user")]
        public async Task<IActionResult> UpdateUser()
        {
            return Ok();
        }
        [HttpPost("tttttttttt")]
        public async Task<IActionResult> SetUserRole(int Id)
        {
            return Ok();
        }
        [HttpDelete("iiiiiiiiiiiiii")]
        public async Task<IActionResult> DeletebyId(int Id)
        {
            return Ok();
        }
        [HttpDelete("uiuooooooooooo")]
        public async Task<IActionResult> DeleteAllUsers()
        {
            return Ok();
        }

    }
}
