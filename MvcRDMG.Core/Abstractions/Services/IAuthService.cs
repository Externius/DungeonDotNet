using MvcRDMG.Core.Abstractions.Services.Models;
using System.Threading.Tasks;

namespace MvcRDMG.Core.Abstractions.Services
{
    public interface IAuthService
    {
        Task<UserModel> LoginAsync(UserModel model);
    }
}
