using RDMG.Core.Domain;
using Shouldly;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace RDMG.Core.Tests.GeneratorTests;

public class DungeonGenerator : DungeonTestBase
{
    public DungeonGenerator(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void CanInit()
    {
        Draw(Dungeon.DungeonTiles);
        Dungeon.RoomDescription.Count.ShouldBe(0);
    }

    [Fact]
    public void CanGenerateRoom()
    {
        Dungeon.GenerateRoom();
        Draw(Dungeon.DungeonTiles);
        Dungeon.RoomDescription.Count.ShouldBe(2);
    }

    [Fact]
    public void CanAddEntryPoint()
    {
        Dungeon.GenerateRoom();
        Dungeon.AddEntryPoint();
        Draw(Dungeon.DungeonTiles);
        var list = Dungeon.DungeonTiles.SelectMany(T => T);
        var match = list.Where(x => x.Texture == Textures.Entry);
        match.ShouldNotBeNull();
    }

    [Fact]
    public void TestGenerateCorridors()
    {
        Dungeon.GenerateRoom();
        Dungeon.AddEntryPoint();
        Dungeon.GenerateCorridors();
        Draw(Dungeon.DungeonTiles);
        var list = Dungeon.DungeonTiles.SelectMany(T => T);
        var match = list.Where(x => x.Texture == Textures.Corridor);
        match.ShouldNotBeNull();
    }

    [Fact]
    public void TestAddDeadEnds()
    {
        Dungeon.GenerateRoom();
        Dungeon.AddEntryPoint();
        Dungeon.GenerateCorridors();
        Dungeon.AddDeadEnds();
        Draw(Dungeon.DungeonTiles);
        var list = Dungeon.DungeonTiles.SelectMany(T => T);
        var match = list.Where(x => x.Texture == Textures.Corridor);
        match.ShouldNotBeNull();
    }

    [Fact]
    public void TestAddTrap()
    {
        Dungeon.GenerateRoom();
        Dungeon.AddEntryPoint();
        Dungeon.GenerateCorridors();
        Dungeon.AddDeadEnds();
        Dungeon.AddCorridorItem(2, Item.Trap);
        Draw(Dungeon.DungeonTiles);
        var list = Dungeon.DungeonTiles.SelectMany(T => T);
        var match = list.Where(x => x.Texture == Textures.Trap);
        match.ShouldNotBeNull();
    }

    [Fact]
    public void TestAddRoamingMonster()
    {
        Dungeon.GenerateRoom();
        Dungeon.AddEntryPoint();
        Dungeon.GenerateCorridors();
        Dungeon.AddDeadEnds();
        Dungeon.AddCorridorItem(2, Item.RoamingMonster);
        Draw(Dungeon.DungeonTiles);
        var list = Dungeon.DungeonTiles.SelectMany(T => T);
        var match = list.Where(x => x.Texture == Textures.RoamingMonster);
        match.ShouldNotBeNull();
    }
}