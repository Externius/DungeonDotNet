using RDMG.Core.Domain;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RDMG.Core.Abstractions.Repository
{
    public interface IDungeonRepository
    {
        Task<IEnumerable<DungeonOption>> GetAllDungeonOptionsAsync(CancellationToken cancellationToken);
        Task<IEnumerable<DungeonOption>> GetAllDungeonOptionsForUserAsync(int userId, CancellationToken cancellationToken);
        Task<IEnumerable<Domain.Dungeon>> GetAllDungeonByOptionNameAsync(string dungeonName, CancellationToken cancellationToken);
        Task<IEnumerable<Domain.Dungeon>> GetAllDungeonsForUserAsync(int userId, CancellationToken cancellationToken);
        Task<Domain.Dungeon> GetDungeonAsync(int id, CancellationToken cancellationToken);
        Task<DungeonOption> GetDungeonOptionAsync(int id, CancellationToken cancellationToken);
        Task<DungeonOption> AddDungeonOptionAsync(DungeonOption dungeonOption, CancellationToken cancellationToken);
        Task<DungeonOption> GetDungeonOptionByNameAsync(string dungeonName, int userId, CancellationToken cancellationToken);
        Task<Domain.Dungeon> AddDungeonAsync(Domain.Dungeon saveddungeon, CancellationToken cancellationToken);
        Task<bool> DeleteDungeonOptionAsync(int id, CancellationToken cancellationToken);
        Task<bool> DeleteSavedDungeonAsync(int id, CancellationToken cancellationToken);
        Task<Domain.Dungeon> UpdateDungeonAsync(Domain.Dungeon dungeon, CancellationToken cancellationToken);
    }
}