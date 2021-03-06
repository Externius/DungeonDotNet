using Microsoft.EntityFrameworkCore;
using MvcRDMG.Core.Domain;
using MvcRDMG.Infrastructure.Extensions;

namespace MvcRDMG.Infrastructure
{
    public class Context : DbContext
    {
        public DbSet<Option> Options { get; set; }
        public DbSet<SavedDungeon> SavedDungeons { get; set; }
        public DbSet<User> Users { get; set; }
        public Context(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Option>()
                .HasIndex(o => new { o.DungeonName, o.UserId })
                .IsUnique();

            modelBuilder.UseEnumStringConverter();
        }
    }
}