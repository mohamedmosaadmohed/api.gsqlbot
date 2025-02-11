using GSQLBOT.Core.DTOs;

namespace GSQLBOT.Services.Interface
{
    public interface IAuthService
    {
       Task<AuthDTOs> RegisterAsync(RegisterDTOs registerDT);
       Task<AuthDTOs> LoginAsync(LoginDTOs loginDTOs);
       Task<string> SendOtp(string email);
    }
}
