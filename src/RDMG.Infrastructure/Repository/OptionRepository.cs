using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RDMG.Core.Abstractions.Data;
using RDMG.Core.Abstractions.Repository;
using RDMG.Core.Domain;

namespace RDMG.Infrastructure.Repository;

public class OptionRepository(IAppDbContext context, IMapper mapper, ILogger<OptionRepository> logger) : IOptionRepository
{
    private readonly IAppDbContext _context = context;
    private readonly ILogger<OptionRepository> _logger = logger;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<Option>> ListAsync(OptionKey? filter = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Options.AsNoTracking();

        if (filter.HasValue)
            query = query.Where(o => o.Key == filter.Value);

        return await query.ToListAsync(cancellationToken);
    }
}