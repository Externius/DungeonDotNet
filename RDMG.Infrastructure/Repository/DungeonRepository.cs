using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RDMG.Core.Abstractions.Repository;
using RDMG.Core.Domain;

namespace RDMG.Infrastructure
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

        public async Task<IEnumerable<Option>> GetAllOptionsAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Options
                                        .OrderBy(d => d.Created)
                                        .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get options from DB", ex);
                return null;
            }
        }

        public async Task<IEnumerable<Option>> GetAllOptionsWithSavedDungeonsAsync(int userId, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Options
                    .Include(d => d.SavedDungeons)
                    .OrderBy(d => d.Created)
                    .Where(d => d.UserId == userId)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get options with saved dungeons from DB", ex);
                return null;
            }
        }

        public async Task<Option> AddDungeonOptionAsync(Option dungeonOption, CancellationToken cancellationToken)
        {
            _context.Add(dungeonOption);
            await _context.SaveChangesAsync(cancellationToken);
            return dungeonOption;
        }

        public async Task<Option> GetSavedDungeonByNameAsync(string dungeonName, int userId, CancellationToken cancellationToken)
        {
            return await _context.Options
                .Include(d => d.SavedDungeons)
                .Where(d => d.DungeonName == dungeonName && d.UserId == userId)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<SavedDungeon> AddSavedDungeonAsync(string dungeonName, SavedDungeon savedDungeon, int userId, CancellationToken cancellationToken)
        {
            var option = await GetSavedDungeonByNameAsync(dungeonName, userId, cancellationToken);
            option.SavedDungeons.Add(savedDungeon);
            _context.SavedDungeons.Add(savedDungeon);
            await _context.SaveChangesAsync(cancellationToken);
            return savedDungeon;
        }

        public async Task<IEnumerable<Option>> GetUserOptionsWithSavedDungeonsAsync(int userId, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Options
                    .Include(d => d.SavedDungeons)
                    .OrderBy(d => d.Created)
                    .Where(d => d.UserId == userId)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get options with saved dungeons from DB", ex);
                return null;
            }
        }
    }
}