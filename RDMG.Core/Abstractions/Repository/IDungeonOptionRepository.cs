using RDMG.Core.Domain;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RDMG.Core.Abstractions.Repository
{
    public interface IDungeonOptionRepository
    {
        Task<IEnumerable<DungeonOption>> GetAllDungeonOptionsAsync(CancellationToken cancellationToken);
        Task<IEnumerable<DungeonOption>> GetAllDungeonOptionsForUserAsync(int userId, CancellationToken cancellationToken);
        Task<DungeonOption> GetDungeonOptionAsync(int id, CancellationToken cancellationToken);
        Task<DungeonOption> AddDungeonOptionAsync(DungeonOption dungeonOption, CancellationToken cancellationToken);
        Task<DungeonOption> GetDungeonOptionByNameAsync(string dungeonName, int userId, CancellationToken cancellationToken);
        Task<bool> DeleteDungeonOptionAsync(int id, CancellationToken cancellationToken);
        Task<DungeonOption> UpdateDungeonOptionAsync(DungeonOption dungeonOption, CancellationToken cancellationToken);
    }
}
