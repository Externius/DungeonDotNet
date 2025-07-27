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

    public async Task<User> CreateAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync(CancellationToken.None);
        return user;
    }

    public async Task<User?> GetAsync(int id)
    {
        return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> UpdateAsync(User user)
    {
        var local = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

        if (local is null)
            return null;
        _mapper.Map(user, local);
        await _context.SaveChangesAsync(CancellationToken.None);

        return local;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var local = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (local is not null)
        {
            local.IsDeleted = true;
            await _context.SaveChangesAsync(CancellationToken.None);
            return true;
        }

        _logger.LogError("Entity not found (User# {id})", id);
        return false;
    }

    public async Task<User?> GetByUsernameAsync(string username, bool? deleted = false)
    {
        var query = _context.Users.AsNoTracking();

        if (deleted.HasValue)
            query = query.Where(x => x.IsDeleted == deleted.Value);

        return await query.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<IEnumerable<User>> ListAsync(bool? deleted = false)
    {
        var query = _context.Users.AsNoTracking();

        if (deleted.HasValue)
            query = query.Where(x => x.IsDeleted == deleted.Value);

        return await query.ToListAsync();
    }
}