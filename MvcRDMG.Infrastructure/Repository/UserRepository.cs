using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MvcRDMG.Core.Abstractions.Repository;
using MvcRDMG.Core.Domain;
using System.Linq;

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
        public User Create(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public User Get(int id)
        {
            return _context.Users.AsNoTracking().FirstOrDefault(u => u.Id == id);
        }

        public User Update(User user)
        {
            var local = _context.Users.FirstOrDefault(u => u.Id == user.Id);

            if (local != null)
            {
                _mapper.Map(user, local);
                _context.SaveChanges();
            }

            return local;
        }
        public bool Delete(int id)
        {
            var local = _context.Users.FirstOrDefault(u => u.Id == id);

            if (local != null)
            {
                local.Deleted = true;
                _context.SaveChanges();
                return true;
            }
            else
            {
                _logger.LogError($" Entity not found (User# {id})");
                return false;
            }

        }

        public User GetByNameAndPass(string userName, string password)
        {
            //TODO hash?
            return _context.Users.FirstOrDefault(u => u.UserName == userName && u.Password == password);
        }
    }
}
