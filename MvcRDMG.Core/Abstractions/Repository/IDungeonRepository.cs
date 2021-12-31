using MvcRDMG.Core.Domain;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MvcRDMG.Core.Abstractions.Repository
{
    public interface IDungeonRepository
    {
        Task<IEnumerable<Option>> GetAllOptionsAsync(CancellationToken cancellationToken);
        Task<IEnumerable<Option>> GetAllOptionsWithSavedDungeonsAsync(int userId, CancellationToken cancellationToken);
        Task AddDungeonOptionAsync(Option dungeonOption, CancellationToken cancellationToken);
        Task<Option> GetSavedDungeonByNameAsync(string dungeonName, int userId, CancellationToken cancellationToken);
        Task AddSavedDungeonAsync(string dungeonName, SavedDungeon saveddungeon, int userId, CancellationToken cancellationToken);
        Task<IEnumerable<Option>> GetUserOptionsWithSavedDungeonsAsync(int userId, CancellationToken cancellationToken);
    }
}