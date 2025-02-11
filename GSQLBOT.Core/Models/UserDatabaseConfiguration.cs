using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSQLBOT.Core.Model
{
    public class UserDatabaseConfiguration
    {
        public int Id { get; set; }
        public int applicationUserId { get; set; }
        public ApplicationUser applicationUser { get; set; }
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DatabaseName { get; set; }
        public int Port { get; set; }
        public int isActive { get; set; }
        public DateTime createdDateAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
