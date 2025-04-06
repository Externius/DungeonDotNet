using RDMG.Core.Domain;

namespace RDMG.Core.Abstractions.Repository;

public interface IUserRepository
{
    Task<User> CreateAsync(User user);
    Task<User?> UpdateAsync(User user);
    Task<User?> GetByUsernameAsync(string username, bool? deleted = false);
    Task<User?> GetAsync(int id);
    Task<IEnumerable<User>> ListAsync(bool? deleted = false);
    Task<bool> DeleteAsync(int id);
}