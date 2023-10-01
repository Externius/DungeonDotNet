using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using RDMG.Core.Abstractions.Repository;
using RDMG.Core.Abstractions.Services;
using RDMG.Core.Abstractions.Services.Models;
using RDMG.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RDMG.Core.Services;

public class OptionService : IOptionService
{
    private readonly IOptionRepository _optionRepository;
    private readonly IMemoryCache _memoryCache;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public OptionService(IMapper mapper,
        IOptionRepository optionRepository,
        IMemoryCache memoryCache,
        ILogger<OptionService> logger)
    {
        _optionRepository = optionRepository;
        _memoryCache = memoryCache;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<OptionModel>> ListOptionsAsync(OptionKey? filter = null, CancellationToken cancellationToken = default)
    {
        try
        {
            if (_memoryCache.TryGetValue(nameof(ListOptionsAsync), out List<OptionModel> cacheEntry))
                return filter.HasValue ? cacheEntry.Where(o => o.Key == filter.Value) : cacheEntry;

            var options = await _optionRepository.ListAsync(null, cancellationToken);
            cacheEntry = options.Select(_mapper.Map<OptionModel>).ToList();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(10));

            _memoryCache.Set(nameof(ListOptionsAsync), cacheEntry, cacheEntryOptions);

            return filter.HasValue ? cacheEntry.Where(o => o.Key == filter.Value) : cacheEntry;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "List options failed.");
            throw;
        }
    }
}