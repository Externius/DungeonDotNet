using Microsoft.EntityFrameworkCore;
using RDMG.Core.Abstractions.Data;
using RDMG.Core.Domain;
using RDMG.Infrastructure.Extensions;

namespace RDMG.Infrastructure.Data;

public class AppDbContext : DbContext, IAppDbContext
{
    public const string DbProvider = "DbProvider";
    public const string Rdmg = "RDMG";
    public const string SqliteContext = "sqlite";
    public const string SqlServerContext = "sqlserver";

    public DbSet<DungeonOption> DungeonOptions { get; set; }
    public DbSet<Dungeon> Dungeons { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Option> Options { get; set; }
    public AppDbContext()
    {

    }
    protected AppDbContext(DbContextOptions options) : base(options)
    {
    }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
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