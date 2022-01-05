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
            var optionsModel = new OptionModel
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
            CancellationToken token = source.Token;
            var result = await service.CreateDungeonOptionAsync(optionsModel, token);
            result.ShouldBeGreaterThan(0);
        }
        [Fact]
        public async void CanAddSavedDungeon()
        {
            using var env = new TestEnvironment();
            var service = env.GetService<IDungeonService>();
            var source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            var optionsModel = new OptionModel
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
            };
            await service.GenerateAsync(optionsModel);
            var saveddungeon = optionsModel.SavedDungeons.ToList().First();
            var result = await service.AddSavedDungeonAsync("Test1", saveddungeon, 1, token);
            result.ShouldBeGreaterThan(1);
        }
    }
}
