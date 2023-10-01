using RDMG.Core.Abstractions.Services.Exceptions;
using RDMG.Core.Abstractions.Services.Models;
using RDMG.Core.Generator;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RDMG.Core.Tests.DungeonServiceTests;

public class Create : DungeonServiceTestBase
{
    [Fact]
    public async Task CreateDungeonOptionAsync_WithOptionModel_ReturnsNewEntityId()
    {
        var optionsModel = new DungeonOptionModel
        {
            DungeonName = "UT Dungeon",
            Created = DateTime.UtcNow,
            ItemsRarity = 1,
            DeadEnd = true,
            DungeonDifficulty = 1,
            DungeonSize = 25,
            MonsterType = "any",
            PartyLevel = 4,
            PartySize = 4,
            TrapPercent = 20,
            RoamingPercent = 0,
            TreasureValue = 1,
            RoomDensity = 10,
            RoomSize = 20,
            Corridor = false,
            UserId = 1
        };
        var source = new CancellationTokenSource();
        var token = source.Token;
        var result = await DungeonService.CreateDungeonOptionAsync(optionsModel, token);
        result.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task AddDungeonAsync_WithDungeonModel_ReturnsNewEntityId()
    {
        var source = new CancellationTokenSource();
        var token = source.Token;

        var optionsModel = (await DungeonService.GetAllDungeonOptionsForUserAsync(1, token)).First();
        var dungeon = await DungeonService.GenerateDungeonAsync(optionsModel);

        var result = await DungeonService.AddDungeonAsync(dungeon, token);
        result.ShouldBeGreaterThan(1);
    }

    [Fact]
    public async Task CreateOrUpdateDungeonAsync_WithInvalidModel_ReturnsServiceAggregateException()
    {
        var source = new CancellationTokenSource();
        var token = source.Token;
        var optionsModel = new DungeonOptionModel
        {
            DungeonName = string.Empty,
            UserId = 0
        };
        var expectedErrors = new List<string>
        {
            string.Format(Resources.Error.RequiredValidation, nameof(DungeonOptionModel.DungeonName)),
            string.Format(Resources.Error.RequiredValidation, nameof(DungeonOptionModel.UserId))
        };

        var act = async () =>
        {
            await DungeonService.CreateOrUpdateDungeonAsync(optionsModel, false, 1, token);
        };

        var result = await act.ShouldThrowAsync<ServiceAggregateException>();
        result.GetInnerExceptions()
                    .Select(se => se.Message)
                    .OrderBy(s => s)
                    .SequenceEqual(expectedErrors.OrderBy(s => s));
    }

    [Fact]
    public async Task CreateOrUpdateDungeonAsync_WithValidNewModel_CreateOptionAndReturnsDungeonModel()
    {
        var source = new CancellationTokenSource();
        var token = source.Token;

        var optionsModel = new DungeonOptionModel
        {
            DungeonName = "UT Dungeon",
            Created = DateTime.UtcNow,
            ItemsRarity = 1,
            DeadEnd = true,
            DungeonDifficulty = 1,
            DungeonSize = 25,
            MonsterType = "any",
            PartyLevel = 4,
            PartySize = 4,
            TrapPercent = 20,
            RoamingPercent = 0,
            TreasureValue = 1,
            RoomDensity = 10,
            RoomSize = 20,
            Corridor = false,
            UserId = 1
        };
        var result = await DungeonService.CreateOrUpdateDungeonAsync(optionsModel, false, 1, token);
        result.ShouldNotBeNull();
        result.RoamingMonsterDescription.ShouldBe(Constants.Empty);
    }

    [Fact]
    public async Task CreateOrUpdateDungeonAsync_WithValidExistingModel_AddsDungeonToExistingOptionAndReturnsDungeonModel()
    {
        var source = new CancellationTokenSource();
        const int userId = 1;
        const int levelNumber = 2;
        var token = source.Token;
        var existingDungeonOption = (await DungeonService.GetAllDungeonOptionsForUserAsync(userId, token)).First();
        var currentDungeonCount = (await DungeonService.ListUserDungeonsByNameAsync(existingDungeonOption.DungeonName, userId, token)).Count;
        var result = await DungeonService.CreateOrUpdateDungeonAsync(existingDungeonOption, true, levelNumber, token);
        var newDungeonCount = (await DungeonService.ListUserDungeonsByNameAsync(existingDungeonOption.DungeonName, userId, token)).Count;
        result.ShouldNotBeNull();
        result.Level.ShouldBe(levelNumber);
        newDungeonCount.ShouldBeGreaterThan(currentDungeonCount);
    }
}