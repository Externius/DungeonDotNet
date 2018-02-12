using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MvcRDMG.Models
{
    public class UserContext : IdentityDbContext<DungeonUser>
    {
        public UserContext(DbContextOptions<UserContext> context) : base(context)
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connString = Startup.Configuration["Data:AuthConnection"];
            optionsBuilder.UseSqlite(connString);
        }
    }
}