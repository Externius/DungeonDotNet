using RDMG.Core.Abstractions.Services.Models;
using RDMG.Core.Domain;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RDMG.Core.Abstractions.Services;

public interface IOptionService
{
    Task<IEnumerable<OptionModel>> ListOptionsAsync(OptionKey? filter = null, CancellationToken cancellationToken = default);
}