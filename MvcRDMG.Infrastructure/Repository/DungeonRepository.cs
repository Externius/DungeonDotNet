using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<Option>> GetAllOptionsAsync()
        {
            try
            {
                return await _context.Options
                                        .OrderBy(d => d.Created)
                                        .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get options from DB", ex);
                return null;
            }
        }

        public async Task<IEnumerable<Option>> GetAllOptionsWithSavedDungeonsAsync(int userId)
        {
            try
            {
                return await _context.Options
                    .Include(d => d.SavedDungeons)
                    .OrderBy(d => d.Created)
                    .Where(d => d.UserId == userId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get options with saved dungeons from DB", ex);
                return null;
            }
        }

        public async Task AddDungeonOptionAsync(Option dungeonOption)
        {
            _context.Add(dungeonOption);
            await _context.SaveChangesAsync();
        }

        public async Task<Option> GetSavedDungeonByNameAsync(string dungeonName, int userId)
        {
            return await _context.Options
                .Include(d => d.SavedDungeons)
                .Where(d => d.DungeonName == dungeonName && d.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task AddSavedDungeonAsync(string dungeonName, SavedDungeon savedDungeon, int userId)
        {
            var dungeon = await GetSavedDungeonByNameAsync(dungeonName, userId);
            dungeon.SavedDungeons.Add(savedDungeon);
            _context.SavedDungeons.Add(savedDungeon);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Option>> GetUserOptionsWithSavedDungeonsAsync(int userId)
        {
            try
            {
                return await _context.Options
                    .Include(d => d.SavedDungeons)
                    .OrderBy(d => d.Created)
                    .Where(d => d.UserId == userId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get options with saved dungeons from DB", ex);
                return null;
            }
        }
    }
}