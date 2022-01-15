using Microsoft.EntityFrameworkCore;
using RDMG.Core.Domain;
using RDMG.Infrastructure.Extensions;

namespace RDMG.Infrastructure
{
    public partial class Context : DbContext
    {
        public DbSet<DungeonOption> DungeonOptions { get; set; }
        public DbSet<Dungeon> Dungeons { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Option> Options { get; set; }
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
            modelBuilder.Entity<DungeonOption>()
                .HasIndex(o => new { o.DungeonName, o.UserId })
                .IsUnique();

            modelBuilder.Entity<Option>()
                .HasIndex(o => new { o.Key, o.Name })
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
            => options.UseSqlite();
#endif
    }
}