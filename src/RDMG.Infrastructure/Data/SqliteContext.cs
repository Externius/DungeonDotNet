using Microsoft.EntityFrameworkCore;
using RDMG.Core.Domain;

namespace RDMG.Infrastructure.Data;
public class SqliteContext : AppDbContext
{
    private const string CurrentTimestamp = "CURRENT_TIMESTAMP";
    public const string HomeToken = "%HOME%";
    public SqliteContext()
    {

    }
    public SqliteContext(DbContextOptions<SqliteContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Dungeon>().Metadata.UseSqlReturningClause(false);
        modelBuilder.Entity<Dungeon>()
            .Property(p => p.Timestamp)
            .IsRowVersion()
            .HasDefaultValueSql(CurrentTimestamp);
        modelBuilder.Entity<DungeonOption>().Metadata.UseSqlReturningClause(false);
        modelBuilder.Entity<DungeonOption>()
            .Property(p => p.Timestamp)
            .IsRowVersion()
            .HasDefaultValueSql(CurrentTimestamp);
        modelBuilder.Entity<Option>().Metadata.UseSqlReturningClause(false);
        modelBuilder.Entity<Option>()
            .Property(p => p.Timestamp)
            .IsRowVersion()
            .HasDefaultValueSql(CurrentTimestamp);
        modelBuilder.Entity<User>().Metadata.UseSqlReturningClause(false);
        modelBuilder.Entity<User>()
            .Property(p => p.Timestamp)
            .IsRowVersion()
            .HasDefaultValueSql(CurrentTimestamp);
    }

#if DEBUG
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite();
#endif
}