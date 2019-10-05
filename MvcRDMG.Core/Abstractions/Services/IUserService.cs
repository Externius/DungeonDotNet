using MvcRDMG.Core.Abstractions.Services.Models;
using System.Collections.Generic;

namespace MvcRDMG.Core.Abstractions.Services
{
    public interface IUserService
    {
        UserModel Get(int id);
        UserModel Update(UserModel model);
        UserModel Create(UserModel model);
        IEnumerable<UserModel> List(bool? deleted = false);
        bool Delete(int id);
        bool Restore(int id);
        UserModel Login(UserModel model);
    }
}
