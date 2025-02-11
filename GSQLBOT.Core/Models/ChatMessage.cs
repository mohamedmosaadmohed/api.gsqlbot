using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GSQLBOT.Core.Model
{
    public enum SenderType { Mode, User }
    public class ChatMessage
    {
        [Key]
        public int Id { get; set; }
        public int ChatId { get; set; }
        public Chat chat { get; set; }
        public string Message { get; set; }
        public SenderType SenderType { get; set; }
        public DateTime createdDate { get; set; } = DateTime.Now;
    }
}
