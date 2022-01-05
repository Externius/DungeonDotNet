using RDMG.Core.Abstractions.Services.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RDMG.Core.Abstractions.Services
{
    public interface IDungeonService
    {
        Task<IEnumerable<OptionModel>> GetAllOptionsAsync(CancellationToken cancellationToken);
        Task<IEnumerable<OptionModel>> GetAllOptionsWithSavedDungeonsAsync(int userId, CancellationToken cancellationToken);
        Task<int> CreateDungeonOptionAsync(OptionModel dungeonOption, CancellationToken cancellationToken);
        Task<OptionModel> GetSavedDungeonByNameAsync(string dungeonName, int userId, CancellationToken cancellationToken);
        Task<int> AddSavedDungeonAsync(string dungeonName, SavedDungeonModel saveddungeon, int userId, CancellationToken cancellationToken);
        Task<IEnumerable<OptionModel>> GetUserOptionsWithSavedDungeonsAsync(int userId, CancellationToken cancellationToken);
        Task<bool> GenerateAsync(OptionModel model);
    }
}
