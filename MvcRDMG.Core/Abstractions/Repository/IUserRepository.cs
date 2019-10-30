using MvcRDMG.Core.Domain;
using System.Collections.Generic;

namespace MvcRDMG.Core.Abstractions.Repository
{
    public interface IUserRepository
    {
        User Create(User user);
        User Update(User user);
        User GetByUsername(string username, bool? deleted = false);
        User Get(int id);
        IEnumerable<User> List(bool? deleted = false);
        bool Delete(int id);
    }
}
