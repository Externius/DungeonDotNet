using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MvcRDMG.Core.Abstractions.Repository;
using MvcRDMG.Core.Abstractions.Services;
using MvcRDMG.Core.Abstractions.Services.Models;
using MvcRDMG.Core.Domain;


namespace MvcRDMG.Core.Services
{
    public class DungeonService : IDungeonService
    {
        private readonly IDungeonRepository _dungeonRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public DungeonService(IMapper mapper, IDungeonRepository dungeonRepository, ILogger<DungeonService> logger)
        {
            _dungeonRepository = dungeonRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task AddDungeonOptionAsync(OptionModel dungeonOption, CancellationToken cancellationToken)
        {
            try
            {
                await _dungeonRepository.AddDungeonOptionAsync(_mapper.Map<Option>(dungeonOption), cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Dungeon AddDungeonOption failed.");
                throw;
            }         
        }

        public async Task AddSavedDungeonAsync(string dungeonName, SavedDungeonModel saveddungeon, int userId, CancellationToken cancellationToken)
        {
            try
            {
                await _dungeonRepository.AddSavedDungeonAsync(dungeonName, _mapper.Map<SavedDungeon>(saveddungeon), userId, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Dungeon AddSavedDungeon failed.");
                throw;
            }
        }

        public async Task<IEnumerable<OptionModel>> GetAllOptionsAsync(CancellationToken cancellationToken)
        {
            try
            {
                var options = await _dungeonRepository.GetAllOptionsAsync(cancellationToken);

                return options.Select(o => _mapper.Map<OptionModel>(o)).ToList();
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Dungeon GetAllOptions failed.");
                throw;
            }
        }

        public async Task<IEnumerable<OptionModel>> GetAllOptionsWithSavedDungeonsAsync(int userId, CancellationToken cancellationToken)
        {
            try
            {
                var options = await _dungeonRepository.GetAllOptionsWithSavedDungeonsAsync(userId, cancellationToken);

                return options.Select(o => _mapper.Map<OptionModel>(o)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"List Applications failed.");
                throw;
            }

        }

        public async Task<OptionModel> GetSavedDungeonByNameAsync(string dungeonName, int userId, CancellationToken cancellationToken)
        {
            try
            {
                return  _mapper.Map<OptionModel>(await _dungeonRepository.GetSavedDungeonByNameAsync(dungeonName, userId, cancellationToken));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"List Applications failed.");
                throw;
            }
        }

        public async Task<IEnumerable<OptionModel>> GetUserOptionsWithSavedDungeonsAsync(int userId, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _dungeonRepository.GetUserOptionsWithSavedDungeonsAsync(userId, cancellationToken);
                
                return result.Select(o => _mapper.Map<OptionModel>(o)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"List Applications failed.");
                throw;
            }
        }
    }
}
