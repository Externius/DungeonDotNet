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

        public async Task<int> CreateDungeonOptionAsync(DungeonOptionModel dungeonOption, CancellationToken cancellationToken)
        {
            try
            {
                var option = await _dungeonRepository.AddDungeonOptionAsync(_mapper.Map<DungeonOption>(dungeonOption), cancellationToken);
                return option.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Create dungeon option failed.");
                throw;
            }
        }

        public async Task<int> AddDungeonAsync(DungeonModel saveddungeon, CancellationToken cancellationToken)
        {
            try
            {
                var dungeon = _mapper.Map<Domain.Dungeon>(saveddungeon);
                var sd = await _dungeonRepository.AddDungeonAsync(dungeon, cancellationToken);
                return sd.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Add dungeon failed.");
                throw;
            }
        }

        public async Task<DungeonModel> GenerateDungeonAsync(DungeonOptionModel model)
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
                    var dungeon = new Generator.Dungeon(dungeonOptions, _dungeonHelper);
                    dungeon.Generate();
                    return await Task.FromResult(
                        new DungeonModel()
                        {
                            DungeonTiles = JsonSerializer.Serialize(dungeon.DungeonTiles),
                            RoomDescription = JsonSerializer.Serialize(dungeon.RoomDescription),
                            TrapDescription = JsonSerializer.Serialize(dungeon.TrapDescription),
                            RoamingMonsterDescription = JsonSerializer.Serialize(dungeon.RoamingMonsterDescription),
                            DungeonOptionId = model.Id
                        }
                    );
                }
                else
                {
                    var dungeonNoCorridor = new DungeonNoCorridor(dungeonWidthAndHeight, dungeonWidthAndHeight, model.DungeonSize, model.RoomSize, _dungeonHelper);
                    dungeonNoCorridor.Generate();
                    return await Task.FromResult(
                        new DungeonModel()
                        {
                            DungeonTiles = JsonSerializer.Serialize(dungeonNoCorridor.DungeonTiles),
                            RoomDescription = JsonSerializer.Serialize(dungeonNoCorridor.RoomDescription),
                            DungeonOptionId = model.Id,
                            TrapDescription = "[]",
                            RoamingMonsterDescription = "[]"
                        }
                    );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<DungeonOptionModel>> GetAllDungeonOptionsAsync(CancellationToken cancellationToken)
        {
            try
            {
                var options = await _dungeonRepository.GetAllDungeonOptionsAsync(cancellationToken);

                return options.Select(o => _mapper.Map<DungeonOptionModel>(o)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Get all dungeon options failed.");
                throw;
            }
        }

        public async Task<IEnumerable<DungeonOptionModel>> GetAllDungeonOptionsForUserAsync(int userId, CancellationToken cancellationToken)
        {
            try
            {
                var options = await _dungeonRepository.GetAllDungeonOptionsForUserAsync(userId, cancellationToken);

                return options.Select(o => _mapper.Map<DungeonOptionModel>(o)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Get all dungeon options for user failed.");
                throw;
            }
        }

        public async Task<DungeonOptionModel> GetDungeonOptionByNameAsync(string dungeonName, int userId, CancellationToken cancellationToken)
        {
            try
            {
                return _mapper.Map<DungeonOptionModel>(await _dungeonRepository.GetDungeonOptionByNameAsync(dungeonName, userId, cancellationToken));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Get dungeon option by name failed.");
                throw;
            }
        }

        public async Task<bool> DeleteDungeonOptionAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                return await _dungeonRepository.DeleteDungeonOptionAsync(id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Delete dungeon option failed.");
                throw;
            }
        }

        public async Task<bool> DeleteDungeonAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                return await _dungeonRepository.DeleteSavedDungeonAsync(id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Delete dungeon failed.");
                throw;
            }
        }

        public async Task<List<DungeonModel>> ListUserDungeonsAsync(int userId, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _dungeonRepository.GetAllDungeonsForUserAsync(userId, cancellationToken);
                return result.Select(d => _mapper.Map<DungeonModel>(d)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"List dungeons for user failed.");
                throw;
            }
        }

        public async Task<List<DungeonModel>> ListDungeonsByNameAsync(string dungeonName, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _dungeonRepository.GetAllDungeonByOptionNameAsync(dungeonName, cancellationToken);
                return result.Select(d => _mapper.Map<DungeonModel>(d)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"List dungeons by name failed.");
                throw;
            }
        }

        public async Task<DungeonModel> GetDungeonAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                return _mapper.Map<DungeonModel>(await _dungeonRepository.GetDungeonAsync(id, cancellationToken));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Get dungeon failed.");
                throw;
            }
        }

        public async Task UpdateDungeonAsync(DungeonModel model, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _dungeonRepository.GetDungeonAsync(model.Id, cancellationToken);
                // TODO use mapper?
                entity.TrapDescription = model.TrapDescription;
                entity.DungeonTiles = model.DungeonTiles;
                entity.RoomDescription = model.RoomDescription;
                entity.RoamingMonsterDescription = model.RoamingMonsterDescription;
                await _dungeonRepository.UpdateDungeonAsync(entity, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Update dungeon failed.");
                throw;
            }
        }

        public async Task<DungeonOptionModel> GetDungeonOptionAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                return _mapper.Map<DungeonOptionModel>(await _dungeonRepository.GetDungeonOptionAsync(id, cancellationToken));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Get dungeon option failed.");
                throw;
            }
        }
    }
}
