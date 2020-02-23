using MvcRDMG.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MvcRDMG.Core.Abstractions.Repository
{
    public interface IDungeonRepository
    {
        Task<IEnumerable<Option>> GetAllOptionsAsync();
        Task<IEnumerable<Option>> GetAllOptionsWithSavedDungeonsAsync(int userId);
        Task AddDungeonOptionAsync(Option dungeonOption);
        Task<Option> GetSavedDungeonByNameAsync(string dungeonName, int userId);
        Task AddSavedDungeonAsync(string dungeonName, SavedDungeon saveddungeon, int userId);
        Task<IEnumerable<Option>> GetUserOptionsWithSavedDungeonsAsync(int userId);
    }
}