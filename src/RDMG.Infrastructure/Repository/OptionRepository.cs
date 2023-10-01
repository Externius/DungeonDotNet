using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RDMG.Core.Abstractions.Data;
using RDMG.Core.Abstractions.Repository;
using RDMG.Core.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RDMG.Infrastructure.Repository;

public class OptionRepository : IOptionRepository
{
    private readonly IAppDbContext _context;
    private readonly ILogger<OptionRepository> _logger;
    private readonly IMapper _mapper;

    public OptionRepository(IAppDbContext context, IMapper mapper, ILogger<OptionRepository> logger)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Option>> ListAsync(OptionKey? filter = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Options.AsNoTracking();

        if (filter.HasValue)
            query = query.Where(o => o.Key == filter.Value);

        return await query.ToListAsync(cancellationToken);
    }
}