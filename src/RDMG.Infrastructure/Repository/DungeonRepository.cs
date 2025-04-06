using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RDMG.Core.Abstractions.Data;
using RDMG.Core.Abstractions.Repository;
using RDMG.Core.Domain;

namespace RDMG.Infrastructure.Repository;

public class DungeonRepository(IAppDbContext context, IMapper mapper, ILogger<DungeonRepository> logger) : IDungeonRepository
{
    private readonly IAppDbContext _context = context;
    private readonly ILogger<DungeonRepository> _logger = logger;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<Dungeon>> GetAllDungeonsForUserAsync(int userId, CancellationToken cancellationToken)
    {
        return await _context.DungeonOptions
                .AsNoTracking()
                .Include(d => d.Dungeons)
                .Where(d => d.UserId == userId)
                .OrderBy(d => d.Created)
                .SelectMany(d => d.Dungeons)
                .ToListAsync(cancellationToken);
    }

    public async Task<Dungeon> AddDungeonAsync(Dungeon savedDungeon, CancellationToken cancellationToken)
    {
        _context.Dungeons.Add(savedDungeon);
        await _context.SaveChangesAsync(cancellationToken);
        return savedDungeon;
    }

    public async Task<bool> DeleteDungeonAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await _context.Dungeons
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        if (entity is not null)
        {
            _context.Dungeons.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        _logger.LogError(" Entity not found (SavedDungeon# {id})", id);
        return false;
    }

    public async Task<IEnumerable<Dungeon>> GetAllDungeonByOptionNameForUserAsync(string dungeonName, int userId, CancellationToken cancellationToken)
    {
        return await _context.DungeonOptions
            .AsNoTracking()
            .Include(d => d.Dungeons)
            .Where(d => d.DungeonName == dungeonName && d.UserId == userId)
            .OrderBy(d => d.Created)
            .SelectMany(d => d.Dungeons)
            .ToListAsync(cancellationToken);
    }

    public async Task<Dungeon?> GetDungeonAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Dungeons
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<Dungeon?> UpdateDungeonAsync(Dungeon dungeon, CancellationToken cancellationToken)
    {
        var local = await _context.Dungeons
            .FirstOrDefaultAsync(d => d.Id == dungeon.Id, cancellationToken);

        if (local is null)
            return null;
        _mapper.Map(dungeon, local);
        await _context.SaveChangesAsync(cancellationToken);

        return local;
    }
}