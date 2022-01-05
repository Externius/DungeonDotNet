using AutoMapper;
using Microsoft.Extensions.Logging;
using RDMG.Core.Abstractions.Dungeon;
using RDMG.Core.Abstractions.Dungeon.Models;
using RDMG.Core.Abstractions.Repository;
using RDMG.Core.Abstractions.Services;
using RDMG.Core.Abstractions.Services.Models;
using RDMG.Core.Domain;
using RDMG.Core.Generator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace RDMG.Core.Services
{
    public class DungeonService : IDungeonService
    {
        private readonly IDungeonRepository _dungeonRepository;
        private readonly IDungeonHelper _dungeonHelper;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private static readonly int dungeonWidthAndHeight = 800;

        public DungeonService(IMapper mapper, 
            IDungeonRepository dungeonRepository,
            IDungeonHelper dungeonHelper,
            ILogger<DungeonService> logger)
        {
            _dungeonRepository = dungeonRepository;
            _dungeonHelper = dungeonHelper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> CreateDungeonOptionAsync(OptionModel dungeonOption, CancellationToken cancellationToken)
        {
            try
            {
                var option = await _dungeonRepository.AddDungeonOptionAsync(_mapper.Map<Option>(dungeonOption), cancellationToken);
                return option.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Dungeon AddDungeonOption failed.");
                throw;
            }         
        }

        public async Task<int> AddSavedDungeonAsync(string dungeonName, SavedDungeonModel saveddungeon, int userId, CancellationToken cancellationToken)
        {
            try
            {
                var sd = await _dungeonRepository.AddSavedDungeonAsync(dungeonName, _mapper.Map<SavedDungeon>(saveddungeon), userId, cancellationToken);
                return sd.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Dungeon AddSavedDungeon failed.");
                throw;
            }
        }

        public async Task<bool> GenerateAsync(OptionModel model)
        {
            _dungeonHelper.Init(model);
            var dungeonOptions = new DungeonOptionsModel
            {
                Size = model.DungeonSize,
                RoomDensity = model.RoomDensity,
                RoomSizePercent = model.RoomSize,
                TrapPercent = model.TrapPercent,
                HasDeadEnds = model.DeadEnd,
                RoamingPercent = model.RoamingPercent
            };
            try
            {
                if (model.Corridor)
                {
                    var dungeon = new Dungeon(dungeonOptions, _dungeonHelper);
                    dungeon.Generate();
                    model.SavedDungeons = new List<SavedDungeonModel>()
                    {
                        new SavedDungeonModel()
                        {
                            DungeonTiles = JsonSerializer.Serialize(dungeon.DungeonTiles),
                            RoomDescription = JsonSerializer.Serialize(dungeon.RoomDescription),
                            TrapDescription = JsonSerializer.Serialize(dungeon.TrapDescription),
                            RoamingMonsterDescription = JsonSerializer.Serialize(dungeon.RoamingMonsterDescription)
                        }
                    };
                    return await Task.FromResult(true);
                }
                else
                {
                    var dungeonNoCorridor = new DungeonNoCorridor(dungeonWidthAndHeight, dungeonWidthAndHeight, model.DungeonSize, model.RoomSize, _dungeonHelper);
                    dungeonNoCorridor.Generate();
                    model.SavedDungeons = new List<SavedDungeonModel>()
                    {
                        new SavedDungeonModel()
                        {
                            DungeonTiles = JsonSerializer.Serialize(dungeonNoCorridor.DungeonTiles),
                            RoomDescription = JsonSerializer.Serialize(dungeonNoCorridor.RoomDescription)
                        }
                    };
                    return await Task.FromResult(true);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
                return await Task.FromResult(false);
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
