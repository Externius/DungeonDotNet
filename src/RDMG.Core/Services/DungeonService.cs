using MapsterMapper;
using Microsoft.Extensions.Logging;
using RDMG.Core.Abstractions.Generator;
using RDMG.Core.Abstractions.Repository;
using RDMG.Core.Abstractions.Services;
using RDMG.Core.Abstractions.Services.Exceptions;
using RDMG.Core.Abstractions.Services.Models;
using RDMG.Core.Domain;
using RDMG.Resources;

namespace RDMG.Core.Services;

public class DungeonService(
    IMapper mapper,
    IDungeonRepository dungeonRepository,
    IDungeonOptionRepository dungeonOptionRepository,
    IDungeon dungeon,
    IDungeonNoCorridor dungeonNcDungeon,
    ILogger<DungeonService> logger) : IDungeonService
{
    private readonly IDungeonRepository _dungeonRepository = dungeonRepository;
    private readonly IDungeonOptionRepository _dungeonOptionRepository = dungeonOptionRepository;
    private readonly IDungeon _dungeon = dungeon;
    private readonly IDungeonNoCorridor _dungeonNcDungeon = dungeonNcDungeon;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger _logger = logger;

    public async Task<int> CreateDungeonOptionAsync(DungeonOptionModel dungeonOption,
        CancellationToken cancellationToken)
    {
        try
        {
            ValidateModel(dungeonOption);
            var option =
                await _dungeonOptionRepository.AddDungeonOptionAsync(_mapper.Map<DungeonOption>(dungeonOption),
                    cancellationToken);
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
        var errors = new List<ServiceException>();
        if (string.IsNullOrWhiteSpace(model.DungeonName))
            errors.Add(new ServiceException(string.Format(Error.RequiredValidation, nameof(model.DungeonName))));
        if (model.UserId == 0)
            errors.Add(new ServiceException(string.Format(Error.RequiredValidation, nameof(model.UserId))));
        if (errors.Count != 0)
            throw new ServiceAggregateException(errors);
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

    public async Task<DungeonModel> CreateOrUpdateDungeonAsync(DungeonOptionModel optionModel, bool addDungeon,
        int level, CancellationToken cancellationToken)
    {
        try
        {
            ValidateModel(optionModel);
            if (addDungeon)
            {
                return await AddDungeonToExistingOptionAsync(optionModel, level, cancellationToken);
            }

            var existingDungeonOption =
                await GetDungeonOptionByNameAsync(optionModel.DungeonName, optionModel.UserId, cancellationToken);
            if (existingDungeonOption is null)
            {
                return await CreateOptionAndAddDungeonToItAsync(optionModel, cancellationToken);
            }

            // regenerate
            var existingDungeons =
                await ListUserDungeonsByNameAsync(optionModel.DungeonName, optionModel.UserId, cancellationToken);
            var oldDungeon = existingDungeons.FirstOrDefault();
            if (oldDungeon is not null)
            {
                return await UpdateExistingDungeonAsync(optionModel, existingDungeonOption, oldDungeon,
                    cancellationToken);
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

    private async Task<DungeonModel> UpdateExistingDungeonAsync(DungeonOptionModel optionModel,
        DungeonOptionModel existingDungeonOption, DungeonModel oldDungeon, CancellationToken cancellationToken)
    {
        var dungeon = await GenerateDungeonAsync(optionModel, existingDungeonOption.Id);
        dungeon.Id = oldDungeon.Id;
        dungeon.Level = oldDungeon.Level;
        await UpdateDungeonAsync(dungeon, cancellationToken);
        return dungeon;
    }

    private async Task<DungeonModel> CreateOptionAndAddDungeonToItAsync(DungeonOptionModel optionModel,
        CancellationToken cancellationToken)
    {
        var createdId = await CreateDungeonOptionAsync(optionModel, cancellationToken);
        var dungeon = await GenerateDungeonAsync(optionModel, createdId);
        dungeon.Level = 1;
        var id = await AddDungeonAsync(dungeon, cancellationToken);
        dungeon.Id = id;
        return dungeon;
    }

    private async Task<DungeonModel> AddDungeonToExistingOptionAsync(DungeonOptionModel optionModel, int level,
        CancellationToken cancellationToken)
    {
        var existingDungeons =
            (await ListUserDungeonsByNameAsync(optionModel.DungeonName, optionModel.UserId, cancellationToken))
            .ToList();
        var dungeon = await GenerateDungeonAsync(optionModel, optionModel.Id);
        dungeon.Level = level;
        if (existingDungeons.Exists(d => d.Level == level))
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
            if (model.Corridor)
                return await Task.FromResult(_dungeon.Generate(model));
            return await Task.FromResult(_dungeonNcDungeon.Generate(model));
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

            return options.Select(_mapper.Map<DungeonOptionModel>);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Get all dungeon options failed.");
            throw;
        }
    }

    public async Task<IEnumerable<DungeonOptionModel>> GetAllDungeonOptionsForUserAsync(int userId,
        CancellationToken cancellationToken)
    {
        try
        {
            var options = await _dungeonOptionRepository.GetAllDungeonOptionsForUserAsync(userId, cancellationToken);

            return options.Select(_mapper.Map<DungeonOptionModel>);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Get all dungeon options for user failed.");
            throw;
        }
    }

    public async Task<DungeonOptionModel?> GetDungeonOptionByNameAsync(string dungeonName, int userId,
        CancellationToken cancellationToken)
    {
        try
        {
            var model = await _dungeonOptionRepository.GetDungeonOptionByNameAsync(dungeonName, userId,
                cancellationToken);
            return model is not null ? _mapper.Map<DungeonOptionModel>(model) : null;
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

    public async Task<IEnumerable<DungeonModel>> ListUserDungeonsAsync(int userId, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _dungeonRepository.GetAllDungeonsForUserAsync(userId, cancellationToken);
            return result.Select(_mapper.Map<DungeonModel>);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "List dungeons for user failed.");
            throw;
        }
    }

    public async Task<IEnumerable<DungeonModel>> ListUserDungeonsByNameAsync(string dungeonName, int userId,
        CancellationToken cancellationToken)
    {
        try
        {
            var result =
                await _dungeonRepository.GetAllDungeonByOptionNameForUserAsync(dungeonName, userId, cancellationToken);
            return result.Select(_mapper.Map<DungeonModel>);
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
            return _mapper.Map<DungeonModel>(await _dungeonRepository.GetDungeonAsync(id, cancellationToken) ??
                                             throw new ServiceException(Error.NotFound));
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
            if (entity is not null)
            {
                _mapper.Map(model, entity);
                await _dungeonRepository.UpdateDungeonAsync(entity, cancellationToken);
            }
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
            return _mapper.Map<DungeonOptionModel>(
                await _dungeonOptionRepository.GetDungeonOptionAsync(id, cancellationToken) ??
                throw new ServiceException(Error.NotFound));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Get dungeon option failed.");
            throw;
        }
    }

    public async Task RenameDungeonAsync(int optionId, int userId, string newName, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _dungeonOptionRepository.GetDungeonOptionAsync(optionId, cancellationToken);
            if (entity is not null)
            {
                entity.DungeonName = newName;
                await _dungeonOptionRepository.UpdateDungeonOptionAsync(entity, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Update dungeon failed.");
            throw;
        }
    }
}