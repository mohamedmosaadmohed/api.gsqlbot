using System.ComponentModel.DataAnnotations;

namespace GSQLBOT.Core.DTOs
{
    public class LoginDTOs
    {
        [Required]
        public string? Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
