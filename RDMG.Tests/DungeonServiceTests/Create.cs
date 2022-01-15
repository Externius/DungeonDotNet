using RDMG.Core.Abstractions.Services;
using RDMG.Core.Abstractions.Services.Models;
using Shouldly;
using System;
using System.Linq;
using System.Threading;
using Xunit;

namespace RDMG.Tests.DungeonServiceTests
{
    public class Create
    {
        [Fact]
        public async void CanCreateDungeonOption()
        {
            using var env = new TestEnvironment();
            var service = env.GetService<IDungeonService>();
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
            var result = await service.CreateDungeonOptionAsync(optionsModel, token);
            result.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async void CanAddDungeon()
        {
            using var env = new TestEnvironment();
            var service = env.GetService<IDungeonService>();
            var source = new CancellationTokenSource();
            var token = source.Token;

            var optionsModel = (await service.GetAllDungeonOptionsForUserAsync(1, token)).First();
            var saveddungeon = await service.GenerateDungeonAsync(optionsModel);

            var result = await service.AddDungeonAsync(saveddungeon, token);
            result.ShouldBeGreaterThan(1);
        }

        [Fact]
        public async void CanCreateOrUpdateDungeon()
        {
            using var env = new TestEnvironment();
            var service = env.GetService<IDungeonService>();
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
            var result = await service.CreateOrUpdateDungeonAsync(optionsModel, false, 1, token);
            result.ShouldNotBeNull();

            var existingDungeonOption = (await service.GetAllDungeonOptionsForUserAsync(optionsModel.UserId, token)).First();
            var dungeonCount = (await service.ListUserDungeonsByNameAsync(existingDungeonOption.DungeonName, optionsModel.UserId, token)).Count;

            result = await service.CreateOrUpdateDungeonAsync(existingDungeonOption, true, 2, token);
            var newDungeonCount = (await service.ListUserDungeonsByNameAsync(existingDungeonOption.DungeonName, optionsModel.UserId, token)).Count;
            result.ShouldNotBeNull();
            result.Level.ShouldBe(2);
            newDungeonCount.ShouldBeGreaterThan(dungeonCount);
        }
    }
}
