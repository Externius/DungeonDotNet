using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MvcRDMG.Core.Abstractions.Repository;
using MvcRDMG.Core.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcRDMG.Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly Context _context;
        private readonly ILogger<UserRepository> _logger;
        private readonly IMapper _mapper;
        public UserRepository(Context context, IMapper mapper, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<User> CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetAsync(int id)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> UpdateAsync(User user)
        {
            var local = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

            if (local != null)
            {
                _mapper.Map(user, local);
               await _context.SaveChangesAsync();
            }

            return local;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var local = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (local != null)
            {
                local.Deleted = true;
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                _logger.LogError($" Entity not found (User# {id})");
                return false;
            }
        }

        public async Task<User> GetByUsernameAsync(string username, bool? deleted = false)
        {
            var query = _context.Users.AsNoTracking();

            if (deleted.HasValue)
                query = query.Where(x => x.Deleted == deleted.Value);

            return await query.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<IEnumerable<User>> ListAsync(bool? deleted = false)
        {
            var query = _context.Users.AsNoTracking();

            if (deleted.HasValue)
                query = query.Where(x => x.Deleted == deleted.Value);

            return await query.ToListAsync();
        }
    }
}
