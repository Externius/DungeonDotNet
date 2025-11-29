using RDMG.Core.Abstractions.Services.Models;
using RDMG.Core.Domain;

namespace RDMG.Core.Abstractions.Services;

public interface IOptionService
{
    Task<OptionModel[]> ListOptionsAsync(OptionKey? filter = null, CancellationToken cancellationToken = default);
}