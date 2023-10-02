using Microsoft.EntityFrameworkCore;
using RDMG.Core.Domain;

namespace RDMG.Infrastructure.Data;
public class SqliteContext : AppDbContext
{
    public SqliteContext()
    {

    }
    public SqliteContext(DbContextOptions<SqliteContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Dungeon>()
            .Property(p => p.Timestamp)
            .IsConcurrencyToken();
        modelBuilder.Entity<DungeonOption>()
            .Property(p => p.Timestamp)
            .IsConcurrencyToken();
        modelBuilder.Entity<Option>()
            .Property(p => p.Timestamp)
            .IsConcurrencyToken();
        modelBuilder.Entity<User>()
            .Property(p => p.Timestamp)
            .IsConcurrencyToken();
    }

#if DEBUG
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite();
#endif
}