using MvcRDMG.Core.Abstractions.Services.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MvcRDMG.Core.Abstractions.Services
{
    public interface IDungeonService
    {
        Task<IEnumerable<OptionModel>> GetAllOptionsAsync(CancellationToken cancellationToken);
        Task<IEnumerable<OptionModel>> GetAllOptionsWithSavedDungeonsAsync(int userId, CancellationToken cancellationToken);
        Task AddDungeonOptionAsync(OptionModel dungeonOption, CancellationToken cancellationToken);
        Task<OptionModel> GetSavedDungeonByNameAsync(string dungeonName, int userId, CancellationToken cancellationToken);
        Task AddSavedDungeonAsync(string dungeonName, SavedDungeonModel saveddungeon, int userId, CancellationToken cancellationToken);
        Task<IEnumerable<OptionModel>> GetUserOptionsWithSavedDungeonsAsync(int userId, CancellationToken cancellationToken);
    }
}
