using Microsoft.EntityFrameworkCore;
using MvcRDMG.Core.Domain;
using MvcRDMG.Infrastructure.Extensions;

namespace MvcRDMG.Infrastructure
{
    public partial class Context : DbContext
    {
        public DbSet<Option> Options { get; set; }
        public DbSet<SavedDungeon> SavedDungeons { get; set; }
        public DbSet<User> Users { get; set; }
        public Context()
        {

        }
        protected Context(DbContextOptions options) : base(options)
        {
        }
        public Context(DbContextOptions<Context> options) : base(options)
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

    public class SqlServerContext : Context
    {
        public SqlServerContext()
        {

        }
        public SqlServerContext(DbContextOptions<SqlServerContext> options) : base(options)
        {

        }

#if DEBUG
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer();
#endif
    }

    public class SqliteContext : Context
    {
        public SqliteContext()
        {

        }
        public SqliteContext(DbContextOptions<SqliteContext> options) : base(options)
        {

        }
#if DEBUG
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("DataSource=dungeons.sqlite");
#endif
    }
}