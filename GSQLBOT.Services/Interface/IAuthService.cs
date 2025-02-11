using GSQLBOT.Core.DTOs;
using Microsoft.AspNetCore.Identity;

namespace GSQLBOT.Services.Interface
{
    public interface IAuthService
    {
       Task<AuthDTOs> RegisterAsync(RegisterDTOs registerDT);
       Task<AuthDTOs> LoginAsync(LoginDTOs loginDTOs);
       Task<bool> SendOtpAsync(string email);
       Task<AuthDTOs> VerifyOtpAsync(string email, string otp, bool isForResetPassword);
       Task<bool> ForgetPasswordAsync(string email);
       Task<AuthDTOs> NewPasswordAsync(string email, string newPassword);
       Task<bool> ChangePasswordAsync(ChangePassDTOs changePassDTOs);
    }
}
