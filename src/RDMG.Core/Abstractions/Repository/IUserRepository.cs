using RDMG.Core.Domain;

namespace RDMG.Core.Abstractions.Repository;

public interface IUserRepository
{
    Task<User> CreateAsync(User user, CancellationToken cancellationToken = default);
    Task<User?> UpdateAsync(User user, CancellationToken cancellationToken = default);

    Task<User?> GetByUsernameAsync(string username, bool? deleted = false,
        CancellationToken cancellationToken = default);

    Task<User?> GetAsync(int id, CancellationToken cancellationToken = default);
    Task<User[]> ListAsync(bool? deleted = false, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}