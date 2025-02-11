using GSQLBOT.Core.DTOs;

namespace GSQLBOT.Services.Interface
{
    public interface IAuthService
    {
       Task<AuthDTOs> RegisterAsync(RegisterDTOs registerDT);
       Task<AuthDTOs> LoginAsync(LoginDTOs loginDTOs);
       Task<bool> SendOtpAsync(string email);
       Task<AuthDTOs> VerifyOtpAsync(string email, string otp);
    }
}
