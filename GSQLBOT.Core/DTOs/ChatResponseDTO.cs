using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSQLBOT.Core.DTOs
{
    public class ChatResponseDTO
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public DateTime CreatedDate { get; set; }
    }

}
