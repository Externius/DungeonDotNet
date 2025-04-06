using RDMG.Core.Domain;

namespace RDMG.Core.Abstractions.Repository;

public interface IOptionRepository
{
    Task<IEnumerable<Option>> ListAsync(OptionKey? filter = null, CancellationToken cancellationToken = default);
}