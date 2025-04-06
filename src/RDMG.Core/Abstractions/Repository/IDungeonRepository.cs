namespace RDMG.Core.Abstractions.Repository;

public interface IDungeonRepository
{
    Task<IEnumerable<Domain.Dungeon>> GetAllDungeonByOptionNameForUserAsync(string dungeonName, int userId, CancellationToken cancellationToken);
    Task<IEnumerable<Domain.Dungeon>> GetAllDungeonsForUserAsync(int userId, CancellationToken cancellationToken);
    Task<Domain.Dungeon?> GetDungeonAsync(int id, CancellationToken cancellationToken);
    Task<Domain.Dungeon> AddDungeonAsync(Domain.Dungeon savedDungeon, CancellationToken cancellationToken);
    Task<bool> DeleteDungeonAsync(int id, CancellationToken cancellationToken);
    Task<Domain.Dungeon?> UpdateDungeonAsync(Domain.Dungeon dungeon, CancellationToken cancellationToken);
}