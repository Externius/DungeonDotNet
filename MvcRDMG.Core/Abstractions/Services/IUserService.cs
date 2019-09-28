using MvcRDMG.Core.Abstractions.Services.Models;

namespace MvcRDMG.Core.Abstractions.Services
{
    public interface IUserService
    {
        UserModel Get(int id);
        UserModel Update(UserModel model);
        UserModel Create(UserModel model);
        bool Delete(int id);
        UserModel Login(UserModel model);
    }
}
