using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MvcRDMG.Generator.Models;

namespace MvcRDMG.Models
{
    public class DungeonContext : DbContext
    {
        public DbSet<Option> Options { get; set; }
        public DbSet<SavedDungeon> SavedDungeon { get; set; }
        public DungeonContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Option>()
                .HasIndex(o => new { o.DungeonName, o.UserName })
                .IsUnique();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connString = Startup.Configuration["Data:DungeonContextConnection"];
            optionsBuilder.UseSqlite(connString);
        }
    }
}