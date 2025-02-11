using GSQLBOT.Core.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GSQLBOT.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public virtual DbSet<Chat> TbChat { get; set; }
        public virtual DbSet<ChatMessage> TbPChatMessage{ get; set; }
        public virtual DbSet<UserDatabaseConfiguration> TbUserDBConfigration { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Customize table names
            modelBuilder.Entity<ApplicationUser>().ToTable("TbUser");
            modelBuilder.Entity<IdentityRole>().ToTable("TbRoles");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("TbUserRoles");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("TbUserClaim");
            // Ignore
            modelBuilder.Ignore<IdentityUserLogin<string>>();
            modelBuilder.Ignore<IdentityUserToken<string>>();
            modelBuilder.Ignore<IdentityRoleClaim<string>>();
        }
    }
}
