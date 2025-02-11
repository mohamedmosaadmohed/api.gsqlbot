using System.ComponentModel.DataAnnotations;

namespace GSQLBOT.Core.Model
{
    public class Chat
    {
        [Key]
        public int Id { get; set; }
        public string? ApplicationUserId { get; set; }
        public ApplicationUser? applicationUser { get; set; }
        public DateTime createdDate { get; set; } = DateTime.Now;
    }
}
