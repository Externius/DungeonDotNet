using AutoMapper;
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

namespace RDMG.Core.Services
{
    public class OptionService : IOptionService
    {
        private readonly IOptionRepository _optionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public OptionService(IMapper mapper,
            IOptionRepository optionRepository,
            ILogger<DungeonService> logger)
        {
            _optionRepository = optionRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<OptionModel>> ListOptionsAsync(CancellationToken cancellationToken, OptionKey? filter = null)
        {
            try
            {
                var options = await _optionRepository.ListAsync(cancellationToken, filter);

                return options.Select(o => _mapper.Map<OptionModel>(o)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"List options failed.");
                throw;
            }
        }
    }
}
