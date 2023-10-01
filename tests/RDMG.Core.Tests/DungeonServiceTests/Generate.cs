using RDMG.Core.Abstractions.Services.Models;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace RDMG.Core.Tests.DungeonServiceTests;

public class Generate : DungeonServiceTestBase
{
    [Fact]
    public async Task GenerateDungeonAsync_WithValidOptionModel_ReturnsDungeonModel()
    {
        var result = await DungeonService.GenerateDungeonAsync(new DungeonOptionModel
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