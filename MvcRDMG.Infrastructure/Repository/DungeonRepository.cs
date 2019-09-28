using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MvcRDMG.Core.Abstractions.Repository;
using MvcRDMG.Core.Domain;

namespace MvcRDMG.Infrastructure
{
    public class DungeonRepository : IDungeonRepository
    {
        private readonly Context _context;
        private readonly ILogger<DungeonRepository> _logger;
        public DungeonRepository(Context context, ILogger<DungeonRepository> logger)
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
        public IEnumerable<Option> GetAllOptionsWithSavedDungeons(int userId)
        {
            try
            {
                return _context.Options
                    .Include(d => d.SavedDungeons)
                    .OrderBy(d => d.Created)
                    .Where(d => d.UserId == userId)
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
        public Option GetSavedDungeonByName(string dungeonName, int userId)
        {
            return _context.Options
                .Include(d => d.SavedDungeons)
                .Where(d => d.DungeonName == dungeonName && d.UserId == userId)
                .FirstOrDefault();
        }
        public void AddSavedDungeon(string dungeonName, SavedDungeon saveddungeon, int userId)
        {
            var dungeon = GetSavedDungeonByName(dungeonName, userId);
            dungeon.SavedDungeons.Add(saveddungeon);
            _context.SavedDungeons.Add(saveddungeon);
        }
        public IEnumerable<Option> GetUserOptionsWithSavedDungeons(int userId)
        {
            try
            {
                return _context.Options
                    .Include(d => d.SavedDungeons)
                    .OrderBy(d => d.Created)
                    .Where(d => d.UserId == userId)
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