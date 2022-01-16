﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RDMG.Core.Abstractions.Repository;
using RDMG.Core.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RDMG.Infrastructure.Repository
{
    public class DungeonOptionRepository : IDungeonOptionRepository
    {
        private readonly Context _context;
        private readonly ILogger<DungeonOptionRepository> _logger;
        private readonly IMapper _mapper;
        public DungeonOptionRepository(Context context, IMapper mapper, ILogger<DungeonOptionRepository> logger)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DungeonOption>> GetAllDungeonOptionsAsync(CancellationToken cancellationToken)
        {
            return await _context.DungeonOptions
                                    .OrderBy(d => d.Created)
                                    .ToListAsync(cancellationToken);

        }

        public async Task<IEnumerable<DungeonOption>> GetAllDungeonOptionsForUserAsync(int userId, CancellationToken cancellationToken)
        {

            return await _context.DungeonOptions
                                    .Where(d => d.UserId == userId)
                                    .OrderBy(d => d.Created)
                                    .ToListAsync(cancellationToken);
        }

        public async Task<DungeonOption> AddDungeonOptionAsync(DungeonOption dungeonOption, CancellationToken cancellationToken)
        {
            _context.Add(dungeonOption);
            await _context.SaveChangesAsync(cancellationToken);
            return dungeonOption;

        }

        public async Task<DungeonOption> GetDungeonOptionByNameAsync(string dungeonName, int userId, CancellationToken cancellationToken)
        {
            return await _context.DungeonOptions
                                    .Where(d => d.DungeonName == dungeonName && d.UserId == userId)
                                    .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> DeleteDungeonOptionAsync(int id, CancellationToken cancellationToken)
        {
            var entity = await _context.DungeonOptions
                                            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

            if (entity != null)
            {
                var dungeons = await _context.Dungeons.Where(sd => sd.DungeonOptionId == entity.Id).ToListAsync(cancellationToken);
                _context.Dungeons.RemoveRange(dungeons);
                _context.DungeonOptions.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
            else
            {
                _logger.LogError($" Entity not found (Option# {id})");
                return false;
            }
        }

        public async Task<DungeonOption> GetDungeonOptionAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.DungeonOptions.FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        }

        public async Task<DungeonOption> UpdateDungeonOptionAsync(DungeonOption dungeonOption, CancellationToken cancellationToken)
        {
            var local = await _context.DungeonOptions.FirstOrDefaultAsync(d => d.Id == dungeonOption.Id, cancellationToken);

            if (local != null)
            {
                _mapper.Map(dungeonOption, local);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return local;
        }
    }
}
