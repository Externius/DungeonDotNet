﻿using RDMG.Core.Domain;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RDMG.Core.Abstractions.Repository;

public interface IOptionRepository
{
    Task<IEnumerable<Option>> ListAsync(OptionKey? filter = null, CancellationToken cancellationToken = default);
}