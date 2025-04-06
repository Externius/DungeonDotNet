using RDMG.Core.Abstractions.Services.Models;

namespace RDMG.Core.Abstractions.Services;

public interface IAuthService
{
    Task<UserModel?> LoginAsync(UserModel model);
}