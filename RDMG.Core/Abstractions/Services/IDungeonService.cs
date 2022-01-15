using RDMG.Core.Abstractions.Services.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RDMG.Core.Abstractions.Services
{
    public interface IDungeonService
    {
        Task<IEnumerable<DungeonOptionModel>> GetAllDungeonOptionsAsync(CancellationToken cancellationToken);
        Task<IEnumerable<DungeonOptionModel>> GetAllDungeonOptionsForUserAsync(int userId, CancellationToken cancellationToken);
        Task<DungeonOptionModel> GetDungeonOptionAsync(int id, CancellationToken cancellationToken);
        Task<DungeonOptionModel> GetDungeonOptionByNameAsync(string dungeonName, int userId, CancellationToken cancellationToken);
        Task<DungeonModel> GetDungeonAsync(int id, CancellationToken cancellationToken);
        Task<DungeonModel> CreateOrUpdateDungeonAsync(DungeonOptionModel optionModel, bool addDungeon, int level, CancellationToken cancellationToken);
        Task UpdateDungeonAsync(DungeonModel model, CancellationToken cancellationToken);
        Task<int> CreateDungeonOptionAsync(DungeonOptionModel dungeonOption, CancellationToken cancellationToken);
        Task<List<DungeonModel>> ListUserDungeonsAsync(int userId, CancellationToken cancellationToken);
        Task<List<DungeonModel>> ListUserDungeonsByNameAsync(string dungeonName, int userId, CancellationToken cancellationToken);
        Task<int> AddDungeonAsync(DungeonModel saveddungeon, CancellationToken cancellationToken);
        Task<bool> DeleteDungeonOptionAsync(int id, CancellationToken cancellationToken);
        Task<bool> DeleteDungeonAsync(int id, CancellationToken cancellationToken);
        Task<DungeonModel> GenerateDungeonAsync(DungeonOptionModel model);
    }
}
