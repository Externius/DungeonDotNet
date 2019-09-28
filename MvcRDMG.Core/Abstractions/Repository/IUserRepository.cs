using MvcRDMG.Core.Domain;

namespace MvcRDMG.Core.Abstractions.Repository
{
    public interface IUserRepository
    {
        User Create(User user);
        User Update(User user);
        User GetByNameAndPass(string userName, string password);
        User Get(int id);
        bool Delete(int id);
    }
}
