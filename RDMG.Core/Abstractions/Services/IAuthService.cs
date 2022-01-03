using RDMG.Core.Abstractions.Services.Models;
using System.Threading.Tasks;

namespace RDMG.Core.Abstractions.Services
{
    public interface IAuthService
    {
        Task<UserModel> LoginAsync(UserModel model);
    }
}
