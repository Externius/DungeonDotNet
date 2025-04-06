using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using RDMG.Core.Domain;

namespace RDMG.Core.Abstractions.Data;

public interface IAppDbContext
{
    public DbSet<DungeonOption> DungeonOptions { get; }
    public DbSet<Dungeon> Dungeons { get; }
    public DbSet<User> Users { get; }
    public DbSet<Option> Options { get; }
    public DatabaseFacade Database { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}