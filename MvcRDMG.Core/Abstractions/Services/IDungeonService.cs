using MvcRDMG.Core.Abstractions.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MvcRDMG.Core.Abstractions.Services
{
    public interface IDungeonService
    {
        Task<IEnumerable<OptionModel>> GetAllOptionsAsync();
        Task<IEnumerable<OptionModel>> GetAllOptionsWithSavedDungeonsAsync(int userId);
        Task AddDungeonOptionAsync(OptionModel dungeonOption);
        Task<OptionModel> GetSavedDungeonByNameAsync(string dungeonName, int userId);
        Task AddSavedDungeonAsync(string dungeonName, SavedDungeonModel saveddungeon, int userId);
        Task<IEnumerable<OptionModel>> GetUserOptionsWithSavedDungeonsAsync(int userId);
    }
}
