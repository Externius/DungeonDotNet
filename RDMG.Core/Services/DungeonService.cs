using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using RDMG.Core.Abstractions.Dungeon.Models;
using RDMG.Core.Abstractions.Repository;
using RDMG.Core.Abstractions.Services;
using RDMG.Core.Abstractions.Services.Models;
using RDMG.Core.Domain;
using RDMG.Core.Generator;
using RDMG.Core.Helpers;

namespace RDMG.Core.Services
{
    public class DungeonService : IDungeonService
    {
        private readonly IDungeonRepository _dungeonRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private static readonly int dungeonWidthAndHeight = 800;

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

        public async Task<bool> GenerateAsync(OptionModel model)
        {
            Utils.Instance.DoorGenerator = new Door();
            Utils.Instance.TrapGenerator = new Trap();
            Utils.Instance.TreasureGenerator = new Treasure();
            Utils.Instance.EncouterGenerator = new Encounter();
            Utils.Instance.PartyLevel = model.PartyLevel;
            Utils.Instance.PartySize = model.PartySize;
            Utils.Instance.TreasureValue = model.TreasureValue;
            Utils.Instance.ItemsRarity = model.ItemsRarity;
            Utils.Instance.DungeonDifficulty = model.DungeonDifficulty;
            Utils.Instance.MonsterType = model.MonsterType;
            Utils.Instance.MonsterList = DeseraliazerJSON<Monster>("5e-SRD-Monsters.json");
            Utils.Instance.TreasureList = DeseraliazerJSON<Treasures>("treasures.json");
            try
            {
                if (model.Corridor)
                {
                    var dungeon = new Dungeon(dungeonWidthAndHeight, dungeonWidthAndHeight, model.DungeonSize, model.RoomDensity, model.RoomSize, model.TrapPercent, model.DeadEnd, model.RoamingPercent);
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
                    var dungeonNoCorridor = new DungeonNoCorridor(dungeonWidthAndHeight, dungeonWidthAndHeight, model.DungeonSize, model.RoomSize);
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

        public static List<T> DeseraliazerJSON<T>(string fileName)
        {
            var json = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/Data/" + fileName);
            return JsonSerializer.Deserialize<List<T>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });
        }
    }
}
