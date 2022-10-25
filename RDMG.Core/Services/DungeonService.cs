using AutoMapper;
using Microsoft.Extensions.Logging;
using RDMG.Core.Abstractions.Dungeon;
using RDMG.Core.Abstractions.Dungeon.Models;
using RDMG.Core.Abstractions.Repository;
using RDMG.Core.Abstractions.Services;
using RDMG.Core.Abstractions.Services.Exceptions;
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
        private readonly IDungeonOptionRepository _dungeonOptionRepository;
        private readonly IDungeonHelper _dungeonHelper;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public DungeonService(IMapper mapper,
            IDungeonRepository dungeonRepository,
            IDungeonOptionRepository dungeonOptionRepository,
            IDungeonHelper dungeonHelper,
            ILogger<DungeonService> logger)
        {
            _dungeonRepository = dungeonRepository;
            _dungeonOptionRepository = dungeonOptionRepository;
            _dungeonHelper = dungeonHelper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> CreateDungeonOptionAsync(DungeonOptionModel dungeonOption, CancellationToken cancellationToken)
        {
            try
            {
                ValidateModel(dungeonOption);
                var option = await _dungeonOptionRepository.AddDungeonOptionAsync(_mapper.Map<DungeonOption>(dungeonOption), cancellationToken);
                return option.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create dungeon option failed.");
                throw;
            }
        }

        private static void ValidateModel(DungeonOptionModel model)
        {
            if (string.IsNullOrWhiteSpace(model.DungeonName))
                throw new ServiceException(Resources.Error.Required);
            if (model.UserId == 0)
                throw new ServiceException(Resources.Error.Required);
        }

        public async Task<int> AddDungeonAsync(DungeonModel savedDungeon, CancellationToken cancellationToken)
        {
            try
            {
                var dungeon = _mapper.Map<Domain.Dungeon>(savedDungeon);
                var sd = await _dungeonRepository.AddDungeonAsync(dungeon, cancellationToken);
                return sd.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Add dungeon failed.");
                throw;
            }
        }

        public async Task<DungeonModel> CreateOrUpdateDungeonAsync(DungeonOptionModel optionModel, bool addDungeon, int level, CancellationToken cancellationToken)
        {
            try
            {
                ValidateModel(optionModel);
                if (addDungeon)
                {
                    return await AddDungeonToExistingOptionAsync(optionModel, level, cancellationToken);
                }

                var existingDungeonOption = await GetDungeonOptionByNameAsync(optionModel.DungeonName, optionModel.UserId, cancellationToken);
                if (existingDungeonOption is null)
                {
                    return await CreateOptionAndAddDungeonToItAsync(optionModel, cancellationToken);
                }

                // regenerate
                var existingDungeons = await ListUserDungeonsByNameAsync(optionModel.DungeonName, optionModel.UserId, cancellationToken);
                var oldDungeon = existingDungeons.FirstOrDefault();
                if (oldDungeon is not null)
                {
                    return await UpdateExistingDungeonAsync(optionModel, existingDungeonOption, oldDungeon, cancellationToken);
                }

                optionModel.Id = existingDungeonOption.Id;
                return await AddDungeonToExistingOptionAsync(optionModel, level, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating dungeon.");
                throw;
            }
        }

        private async Task<DungeonModel> UpdateExistingDungeonAsync(DungeonOptionModel optionModel, DungeonOptionModel existingDungeonOption, DungeonModel oldDungeon, CancellationToken cancellationToken)
        {
            var dungeon = await GenerateDungeonAsync(optionModel, existingDungeonOption.Id);
            dungeon.Id = oldDungeon.Id;
            dungeon.Level = oldDungeon.Level;
            await UpdateDungeonAsync(dungeon, cancellationToken);
            return dungeon;
        }

        private async Task<DungeonModel> CreateOptionAndAddDungeonToItAsync(DungeonOptionModel optionModel, CancellationToken cancellationToken)
        {
            await CreateDungeonOptionAsync(optionModel, cancellationToken);
            var created = await GetDungeonOptionByNameAsync(optionModel.DungeonName, optionModel.UserId, cancellationToken);
            var dungeon = await GenerateDungeonAsync(optionModel, created.Id);
            dungeon.Level = 1;
            var id = await AddDungeonAsync(dungeon, cancellationToken);
            dungeon.Id = id;
            return dungeon;
        }

        private async Task<DungeonModel> AddDungeonToExistingOptionAsync(DungeonOptionModel optionModel, int level, CancellationToken cancellationToken)
        {
            var existingDungeons = await ListUserDungeonsByNameAsync(optionModel.DungeonName, optionModel.UserId, cancellationToken);
            var dungeon = await GenerateDungeonAsync(optionModel, optionModel.Id);
            dungeon.Level = level;
            if (existingDungeons.Any(d => d.Level == level))
            {
                dungeon.Id = existingDungeons.First(dm => dm.Level == level).Id;
                await UpdateDungeonAsync(dungeon, cancellationToken);
            }
            else
            {
                dungeon.Id = await AddDungeonAsync(dungeon, cancellationToken);
            }
            return dungeon;
        }

        private async Task<DungeonModel> GenerateDungeonAsync(DungeonOptionModel optionModel, int optionId)
        {
            var dungeon = await GenerateDungeonAsync(optionModel);
            dungeon.DungeonOptionId = optionId;
            return dungeon;
        }

        public async Task<DungeonModel> GenerateDungeonAsync(DungeonOptionModel model)
        {
            try
            {
                ValidateModel(model);
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

                var dungeonNoCorridor = new DungeonNoCorridor(dungeonOptions.Width, dungeonOptions.Height, model.DungeonSize, model.RoomSize, _dungeonHelper);
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
            catch (Exception ex)
            {
                _logger.LogError("Error: {message}", ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<DungeonOptionModel>> GetAllDungeonOptionsAsync(CancellationToken cancellationToken)
        {
            try
            {
                var options = await _dungeonOptionRepository.GetAllDungeonOptionsAsync(cancellationToken);

                return options.Select(o => _mapper.Map<DungeonOptionModel>(o)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get all dungeon options failed.");
                throw;
            }
        }

        public async Task<IEnumerable<DungeonOptionModel>> GetAllDungeonOptionsForUserAsync(int userId, CancellationToken cancellationToken)
        {
            try
            {
                var options = await _dungeonOptionRepository.GetAllDungeonOptionsForUserAsync(userId, cancellationToken);

                return options.Select(o => _mapper.Map<DungeonOptionModel>(o)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get all dungeon options for user failed.");
                throw;
            }
        }

        public async Task<DungeonOptionModel> GetDungeonOptionByNameAsync(string dungeonName, int userId, CancellationToken cancellationToken)
        {
            try
            {
                return _mapper.Map<DungeonOptionModel>(await _dungeonOptionRepository.GetDungeonOptionByNameAsync(dungeonName, userId, cancellationToken));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get dungeon option by name failed.");
                throw;
            }
        }

        public async Task<bool> DeleteDungeonOptionAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                return await _dungeonOptionRepository.DeleteDungeonOptionAsync(id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete dungeon option failed.");
                throw;
            }
        }

        public async Task<bool> DeleteDungeonAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                return await _dungeonRepository.DeleteDungeonAsync(id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete dungeon failed.");
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
                _logger.LogError(ex, "List dungeons for user failed.");
                throw;
            }
        }

        public async Task<List<DungeonModel>> ListUserDungeonsByNameAsync(string dungeonName, int userId, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _dungeonRepository.GetAllDungeonByOptionNameForUserAsync(dungeonName, userId, cancellationToken);
                return result.Select(d => _mapper.Map<DungeonModel>(d)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "List dungeons by name failed.");
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
                _logger.LogError(ex, "Get dungeon failed.");
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
                entity.Level = model.Level;
                await _dungeonRepository.UpdateDungeonAsync(entity, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update dungeon failed.");
                throw;
            }
        }

        public async Task<DungeonOptionModel> GetDungeonOptionAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                return _mapper.Map<DungeonOptionModel>(await _dungeonOptionRepository.GetDungeonOptionAsync(id, cancellationToken));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get dungeon option failed.");
                throw;
            }
        }

        public async Task RenameDungeonAsync(int id, int userId, string newName, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _dungeonOptionRepository.GetDungeonOptionAsync(id, cancellationToken);
                entity.DungeonName = newName;
                await _dungeonOptionRepository.UpdateDungeonOptionAsync(entity, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update dungeon failed.");
                throw;
            }
        }
    }
}
