using GSQLBOT.Core.DTOs;
using GSQLBOT.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GSQLBOT.Presentation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDTOs registerDTOs)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(registerDTOs);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTOs loginDTOs)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(loginDTOs);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }
        [HttpPost("forgetpassword")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            return Ok();
        }
        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp(string email)
        {
            if (await _authService.SendOtp(email) is not null)
                return Ok("Otp Send Successfully");
            else
                return BadRequest("Error");
        }
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp(string email,string otp)
        {
            return Ok();
        }
        [HttpPost("newpassword")]
        public async Task<IActionResult> NewPassword(string email)
        {
            return Ok();
        }
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePassDTOs changePassDTOs)
        {
            return Ok();
        }
    }
}
