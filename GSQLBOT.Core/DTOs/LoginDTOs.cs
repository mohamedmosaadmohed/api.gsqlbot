using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
