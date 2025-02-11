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
                return BadRequest(new { Message = "Invalid input data",
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) });

            var result = await _authService.RegisterAsync(registerDTOs);

            if (!result.IsAuthenticated)
                return BadRequest(new { Message = result.Message });

            return Ok(new { Message = result.Message, Email = result.Email });
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
        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtpAsync([FromQuery] string email)
        {
            var success = await _authService.SendOtpAsync(email);
            if (!success) return BadRequest("Failed to send OTP. Please try again.");

            return Ok("OTP sent successfully.");
        }
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtpAsync([FromQuery] string email, [FromQuery] string otp)
        {
            var result = await _authService.VerifyOtpAsync(email, otp);
            if (!result.IsAuthenticated) return BadRequest(result.Message);

            return Ok(result);
        }
        [HttpPost("forgetpassword")]
        public async Task<IActionResult> ForgetPassword(string email)
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
