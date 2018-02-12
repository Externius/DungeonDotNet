using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MvcRDMG.Models
{
    public class DungeonRepository : IDungeonRepository
    {
        private DungeonContext _context;
        private ILogger<DungeonRepository> _logger;
        public DungeonRepository(DungeonContext context, ILogger<DungeonRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public IEnumerable<Option> GetAllOptions()
        {
            try
            {
                return _context.Options
                    .OrderBy(d => d.Created)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get options from DB", ex);
                return null;
            }
        }
        public IEnumerable<Option> GetAllOptionsWithSavedDungeons(string userName)
        {
            try
            {
                return _context.Options
                    .Include(d => d.SavedDungeons)
                    .OrderBy(d => d.Created)
                    .Where(d => d.UserName == userName)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get options with saved dungeons from DB", ex);
                return null;
            }
        }
        public void AddDungeonOption(Option dungeonOption)
        {
            _context.Add(dungeonOption);
        }
        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
        public Option GetSavedDungeonByName(string dungeonName, string userName)
        {
            return _context.Options
                .Include(d => d.SavedDungeons)
                .Where(d => d.DungeonName == dungeonName && d.UserName == userName)
                .FirstOrDefault();
        }
        public void AddSavedDungeon(string dungeonName, SavedDungeon saveddungeon, string userName)
        {
            var dungeon = GetSavedDungeonByName(dungeonName, userName);
            dungeon.SavedDungeons.Add(saveddungeon);
            _context.SavedDungeon.Add(saveddungeon);
        }
        public IEnumerable<Option> GetUserOptionsWithSavedDungeons(string userName)
        {
            try
            {
                return _context.Options
                    .Include(d => d.SavedDungeons)
                    .OrderBy(d => d.Created)
                    .Where(d => d.UserName == userName)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get options with saved dungeons from DB", ex);
                return null;
            }
        }
    }
}