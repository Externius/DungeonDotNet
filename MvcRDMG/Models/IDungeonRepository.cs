using System.Collections.Generic;

namespace MvcRDMG.Models
{
    public interface IDungeonRepository
    {
        IEnumerable<Option> GetAllOptions();
        IEnumerable<Option> GetAllOptionsWithSavedDungeons(string userName);
        void AddDungeonOption(Option dungeonOption);
        bool SaveAll();
        Option GetSavedDungeonByName(string dungeonName, string userName);
        void AddSavedDungeon(string dungeonName, SavedDungeon saveddungeon, string userName);
        IEnumerable<Option> GetUserOptionsWithSavedDungeons(string userName);
    }
}