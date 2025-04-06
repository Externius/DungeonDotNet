using RDMG.Core.Domain;

namespace RDMG.Core.Abstractions.Repository;

public interface IDungeonOptionRepository
{
    Task<IEnumerable<DungeonOption>> GetAllDungeonOptionsAsync(CancellationToken cancellationToken);
    Task<IEnumerable<DungeonOption>> GetAllDungeonOptionsForUserAsync(int userId, CancellationToken cancellationToken);
    Task<DungeonOption?> GetDungeonOptionAsync(int id, CancellationToken cancellationToken);
    Task<DungeonOption> AddDungeonOptionAsync(DungeonOption dungeonOption, CancellationToken cancellationToken);
    Task<DungeonOption?> GetDungeonOptionByNameAsync(string dungeonName, int userId, CancellationToken cancellationToken);
    Task<bool> DeleteDungeonOptionAsync(int id, CancellationToken cancellationToken);
    Task<DungeonOption?> UpdateDungeonOptionAsync(DungeonOption dungeonOption, CancellationToken cancellationToken);
}