using MvcRDMG.Core.Abstractions.Services.Models;
using System.Collections.Generic;

namespace MvcRDMG.Core.Abstractions.Services
{
    public interface IDungeonService
    {
        IEnumerable<OptionModel> GetAllOptions();
        IEnumerable<OptionModel> GetAllOptionsWithSavedDungeons(int userId);
        void AddDungeonOption(OptionModel dungeonOption);
        bool SaveAll();
        OptionModel GetSavedDungeonByName(string dungeonName, int userId);
        void AddSavedDungeon(string dungeonName, SavedDungeonModel saveddungeon, int userId);
        IEnumerable<OptionModel> GetUserOptionsWithSavedDungeons(int userId);
    }
}
