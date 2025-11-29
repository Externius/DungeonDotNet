using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RDMG.Core.Abstractions.Data;
using RDMG.Core.Abstractions.Repository;
using RDMG.Core.Domain;

namespace RDMG.Infrastructure.Repository;

public class UserRepository(IAppDbContext context, IMapper mapper, ILogger<UserRepository> logger) : IUserRepository
{
    private readonly IAppDbContext _context = context;
    private readonly ILogger<UserRepository> _logger = logger;
    private readonly IMapper _mapper = mapper;

    public async Task<User> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);
        return user;
    }

    public async Task<User?> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken: cancellationToken);
    }

    public async Task<User?> UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        var local = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id,
            cancellationToken: cancellationToken);

        if (local is null)
            return null;
        _mapper.Map(user, local);
        await _context.SaveChangesAsync(cancellationToken);

        return local;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var local = await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken: cancellationToken);

        if (local is not null)
        {
            local.IsDeleted = true;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        _logger.LogError("Entity not found (User# {id})", id);
        return false;
    }

    public async Task<User?> GetByUsernameAsync(string username, bool? deleted = false,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Users.AsNoTracking();

        if (deleted.HasValue)
            query = query.Where(x => x.IsDeleted == deleted.Value);

        return await query.FirstOrDefaultAsync(u => u.Username == username, cancellationToken: cancellationToken);
    }

    public async Task<User[]> ListAsync(bool? deleted = false, CancellationToken cancellationToken = default)
    {
        var query = _context.Users.AsNoTracking();

        if (deleted.HasValue)
            query = query.Where(x => x.IsDeleted == deleted.Value);

        return await query.ToArrayAsync(cancellationToken);
    }
}