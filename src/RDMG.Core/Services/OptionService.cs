using MapsterMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using RDMG.Core.Abstractions.Repository;
using RDMG.Core.Abstractions.Services;
using RDMG.Core.Abstractions.Services.Models;
using RDMG.Core.Domain;

namespace RDMG.Core.Services;

public class OptionService(
    IMapper mapper,
    IOptionRepository optionRepository,
    IMemoryCache memoryCache,
    ILogger<OptionService> logger) : IOptionService
{
    private readonly IOptionRepository _optionRepository = optionRepository;
    private readonly IMemoryCache _memoryCache = memoryCache;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger _logger = logger;

    public async Task<OptionModel[]> ListOptionsAsync(OptionKey? filter = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (_memoryCache.TryGetValue(nameof(ListOptionsAsync), out OptionModel[]? cacheEntry))
                return filter.HasValue
                    ? cacheEntry?.Where(o => o.Key == filter.Value).ToArray() ?? []
                    : cacheEntry ?? [];

            var options = await _optionRepository.ListAsync(null, cancellationToken);
            cacheEntry = [.. options.Select(_mapper.Map<OptionModel>)];

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(10));

            _memoryCache.Set(nameof(ListOptionsAsync), cacheEntry, cacheEntryOptions);

            return filter.HasValue ? [.. cacheEntry.Where(o => o.Key == filter.Value)] : cacheEntry;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "List options failed.");
            throw;
        }
    }
}