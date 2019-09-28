using MvcRDMG.Core.Domain;
using System.Collections.Generic;

namespace MvcRDMG.Core.Abstractions.Repository
{
    public interface IDungeonRepository
    {
        IEnumerable<Option> GetAllOptions();
        IEnumerable<Option> GetAllOptionsWithSavedDungeons(int userId);
        void AddDungeonOption(Option dungeonOption);
        bool SaveAll();
        Option GetSavedDungeonByName(string dungeonName, int userId);
        void AddSavedDungeon(string dungeonName, SavedDungeon saveddungeon, int userId);
        IEnumerable<Option> GetUserOptionsWithSavedDungeons(int userId);
    }
}