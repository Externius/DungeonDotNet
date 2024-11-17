using RDMG.Core.Abstractions.Generator;
using RDMG.Core.Abstractions.Generator.Models;
using RDMG.Core.Domain;
using Shouldly;
using System.Linq;
using Xunit;

namespace RDMG.Core.Tests.DungeonHelperTests;

public class Get
{
    private readonly IDungeonHelper _dungeonHelper;
    private readonly IDungeonNoCorridor _dungeonNoCorridor;

    public Get()
    {
        using var env = new TestEnvironment();
        _dungeonHelper = env.GetService<IDungeonHelper>();
        _dungeonNoCorridor = env.GetNcDungeon();
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(1, 10)]
    [InlineData(3, 235)]
    [InlineData(11, 329)]
    public void GetRandomInt_WithTwoInteger_ReturnsIntBetweenThem(int min, int max)
    {
        var result = _dungeonHelper.GetRandomInt(min, max);

        result.ShouldBeGreaterThan(min - 1);
        result.ShouldBeLessThan(max);
    }

    public static TheoryData<DungeonTile> GetNcTestDoorTiles()
    {
        return
        [
            new DungeonTile(2, 2, 2, 2, 50, 50, Texture.NoCorridorDoor),
            new DungeonTile(5, 5, 2, 2, 50, 50, Texture.NoCorridorDoorTrapped),
            new DungeonTile(5, 2, 2, 2, 50, 50, Texture.NoCorridorDoorLocked)
        ];
    }

    [Theory]
    [MemberData(nameof(GetNcTestDoorTiles), MemberType = typeof(Get))]
    public void GetNcDoor_WithValidNCCorridorDoorDungeonTile_ReturnsDoorDescription(DungeonTile dungeonTile)
    {
        var result = _dungeonHelper.GetNcDoor(dungeonTile);
        result.ShouldNotBeEmpty();
    }

    [Fact]
    public void GetNcDoorDescription_WithValidParameters_ReturnsNcDoorDescription()
    {
        _dungeonNoCorridor.AddFirstRoom();
        var list = _dungeonNoCorridor.DungeonTiles.SelectMany(T => T);
        var match = list.Where(x => x.Texture == Texture.Room);
        var result = _dungeonHelper.GetNcDoorDescription(_dungeonNoCorridor.DungeonTiles, match);
        result.ShouldNotBeNull();
    }

    [Theory]
    [InlineData(20)]
    public void GetTreasure_ReturnsTreasure(int testRunCount)
    {
        var result = new string[testRunCount];
        for (var i = 0; i < testRunCount; i++)
        {
            result[i] = _dungeonHelper.GetTreasure();
        }

        result.ShouldAllBe(s => !string.IsNullOrWhiteSpace(s));
    }

    [Theory]
    [InlineData(20)]
    public void GetMonsterDescription_ReturnsMonsterDescription(int testRunCount)
    {
        var result = new string[testRunCount];
        for (var i = 0; i < testRunCount; i++)
        {
            result[i] = _dungeonHelper.GetMonsterDescription();
        }

        result.ShouldAllBe(s => !string.IsNullOrWhiteSpace(s));
    }

    [Theory]
    [InlineData(20)]
    public void GetRandomTrapDescription_WithRandomDoorParameter_ReturnsDoorOrTrapDescription(int testRunCount)
    {
        var result = new string[testRunCount];
        for (var i = 0; i < testRunCount; i++)
        {
            result[i] = _dungeonHelper.GetRandomTrapDescription(i % 2 == 0);
        }

        result.ShouldAllBe(s => !string.IsNullOrWhiteSpace(s));
    }

    [Theory]
    [InlineData(20)]
    public void GetRoamingMonsterDescription_ReturnsRoamingMonsterDescription(int testRunCount)
    {
        var result = new string[testRunCount];
        for (var i = 0; i < testRunCount; i++)
        {
            result[i] = _dungeonHelper.GetRoamingMonsterDescription();
        }

        result.ShouldAllBe(s => !string.IsNullOrWhiteSpace(s));
    }
}