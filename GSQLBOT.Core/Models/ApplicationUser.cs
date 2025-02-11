using BGSQLBOTOT.Core.Model;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GSQLBOT.Core.Model
{
    public class ApplicationUser : IdentityUser
    {
        [Required, MaxLength(450)]
        public string FirstName { get; set; }

        [Required, MaxLength(100)]
        public string LastName { get; set; }
        [MinLength(6),MaxLength(6)]
        public string? OTP { get; set; }
        public int SaveChat { get; set; } = 1;
        public DateTime CreateOn { get; set; } = DateTime.UtcNow;
        public List<TbRefreshToken> RefreshTokens { get; set; }
    }
}
