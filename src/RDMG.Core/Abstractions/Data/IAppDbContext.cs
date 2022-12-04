using Microsoft.EntityFrameworkCore;
using RDMG.Core.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace RDMG.Core.Abstractions.Data;

public interface IAppDbContext
{
    public DbSet<DungeonOption> DungeonOptions { get; }
    public DbSet<Dungeon> Dungeons { get; }
    public DbSet<User> Users { get; }
    public DbSet<Option> Options { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}