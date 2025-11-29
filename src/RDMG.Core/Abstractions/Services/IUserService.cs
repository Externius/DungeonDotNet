using RDMG.Core.Abstractions.Services.Models;

namespace RDMG.Core.Abstractions.Services;

public interface IUserService
{
    Task<UserModel> GetAsync(int id, CancellationToken cancellationToken = default);
    Task UpdateAsync(UserModel model, CancellationToken cancellationToken = default);
    Task<int> CreateAsync(UserModel model, CancellationToken cancellationToken = default);
    Task<UserModel[]> ListAsync(bool? deleted = false, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> RestoreAsync(int id, CancellationToken cancellationToken = default);
    Task ChangePasswordAsync(ChangePasswordModel model, CancellationToken cancellationToken = default);
}