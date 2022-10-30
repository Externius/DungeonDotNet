using RDMG.Core.Abstractions.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RDMG.Core.Abstractions.Services;

public interface IUserService
{
    Task<UserModel> GetAsync(int id);
    Task UpdateAsync(UserModel model);
    Task<int> CreateAsync(UserModel model);
    Task<IEnumerable<UserModel>> ListAsync(bool? deleted = false);
    Task<bool> DeleteAsync(int id);
    Task<bool> RestoreAsync(int id);
    Task ChangePasswordAsync(ChangePasswordModel model);
}