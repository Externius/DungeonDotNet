using RDMG.Core.Domain;
using Shouldly;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace RDMG.Tests.GeneratorTests;

public class DungeonGenerator : DungeonTestBase
{
    public DungeonGenerator(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void CanInit()
    {
        using var env = new TestEnvironment();
        var dungeon = env.GetDungeon();
        Draw(dungeon.DungeonTiles);
        dungeon.RoomDescription.Count.ShouldBe(0);
    }

    [Fact]
    public void CanGenerateRoom()
    {
        using var env = new TestEnvironment();
        var dungeon = env.GetDungeon();
        dungeon.GenerateRoom();
        Draw(dungeon.DungeonTiles);
        dungeon.RoomDescription.Count.ShouldBe(2);
    }

    [Fact]
    public void CanAddEntryPoint()
    {
        using var env = new TestEnvironment();
        var dungeon = env.GetDungeon();
        dungeon.GenerateRoom();
        dungeon.AddEntryPoint();
        Draw(dungeon.DungeonTiles);
        var list = dungeon.DungeonTiles.SelectMany(T => T).ToList();
        var match = list.Where(x => x.Texture == Textures.Entry);
        match.ShouldNotBeNull();
    }
    [Fact]
    public void TestGenerateCorridors()
    {
        using var env = new TestEnvironment();
        var dungeon = env.GetDungeon();
        dungeon.GenerateRoom();
        dungeon.AddEntryPoint();
        dungeon.GenerateCorridors();
        Draw(dungeon.DungeonTiles);
        var list = dungeon.DungeonTiles.SelectMany(T => T).ToList();
        var match = list.Where(x => x.Texture == Textures.Corridor);
        match.ShouldNotBeNull();
    }
    [Fact]
    public void TestAddDeadEnds()
    {
        using var env = new TestEnvironment();
        var dungeon = env.GetDungeon();
        dungeon.GenerateRoom();
        dungeon.AddEntryPoint();
        dungeon.GenerateCorridors();
        dungeon.AddDeadEnds();
        Draw(dungeon.DungeonTiles);
        var list = dungeon.DungeonTiles.SelectMany(T => T).ToList();
        var match = list.Where(x => x.Texture == Textures.Corridor);
        match.ShouldNotBeNull();
    }
    [Fact]
    public void TestAddTrap()
    {
        using var env = new TestEnvironment();
        var dungeon = env.GetDungeon();
        dungeon.GenerateRoom();
        dungeon.AddEntryPoint();
        dungeon.GenerateCorridors();
        dungeon.AddDeadEnds();
        dungeon.AddCorridorItem(2, Item.Trap);
        Draw(dungeon.DungeonTiles);
        var list = dungeon.DungeonTiles.SelectMany(T => T).ToList();
        var match = list.Where(x => x.Texture == Textures.Trap);
        match.ShouldNotBeNull();
    }
    [Fact]
    public void TestAddRoamingMonster()
    {
        using var env = new TestEnvironment();
        var dungeon = env.GetDungeon();
        dungeon.GenerateRoom();
        dungeon.AddEntryPoint();
        dungeon.GenerateCorridors();
        dungeon.AddDeadEnds();
        dungeon.AddCorridorItem(2, Item.RoamingMonster);
        Draw(dungeon.DungeonTiles);
        var list = dungeon.DungeonTiles.SelectMany(T => T).ToList();
        var match = list.Where(x => x.Texture == Textures.RoamingMonster);
        match.ShouldNotBeNull();
    }
}