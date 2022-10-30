using RDMG.Core.Abstractions.Services;
using RDMG.Core.Abstractions.Services.Models;
using Shouldly;
using System;
using Xunit;

namespace RDMG.Tests.DungeonServiceTests;

public class Generate
{
    [Fact]
    public async void CanGenerate()
    {
        using var env = new TestEnvironment();
        var service = env.GetService<IDungeonService>();
        var result = await service.GenerateDungeonAsync(new DungeonOptionModel
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
        });

        result.DungeonTiles.ShouldNotBeNull();
    }
}