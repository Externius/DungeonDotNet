using RDMG.Core.Abstractions.Services.Models;
using RDMG.Core.Domain;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RDMG.Core.Abstractions.Services;

public interface IOptionService
{
    Task<List<OptionModel>> ListOptionsAsync(CancellationToken cancellationToken, OptionKey? filter = null);
}