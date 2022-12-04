using RDMG.Core.Domain;
using Shouldly;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace RDMG.Core.Tests.GeneratorTests;

public class DungeonNoCorridorGenerator : DungeonTestBase
{
    public DungeonNoCorridorGenerator(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void CanInit()
    {
        using var env = new TestEnvironment();
        var dungeonNoCorridor = env.GetNcDungeon();
        Draw(dungeonNoCorridor.DungeonTiles);
        dungeonNoCorridor.RoomDescription.Count.ShouldBe(0);
    }

    [Fact]
    public void TestAddFirstRoom()
    {
        using var env = new TestEnvironment();
        var dungeonNoCorridor = env.GetNcDungeon();
        dungeonNoCorridor.AddFirstRoom();
        Draw(dungeonNoCorridor.DungeonTiles);
        var list = dungeonNoCorridor.DungeonTiles.SelectMany(T => T).ToList();
        var match = list.Where(x => x.Texture == Textures.Room);
        match.ShouldNotBeNull();
    }

    [Fact]
    public void TestFillRoomToDoor()
    {
        using var env = new TestEnvironment();
        var dungeonNoCorridor = env.GetNcDungeon();
        dungeonNoCorridor.AddFirstRoom();
        dungeonNoCorridor.FillRoomToDoor();
        Draw(dungeonNoCorridor.DungeonTiles);
        dungeonNoCorridor.OpenDoorList.Count.ShouldBe(0);
    }

    [Fact]
    public void TestAddEntryPoint()
    {
        using var env = new TestEnvironment();
        var dungeonNoCorridor = env.GetNcDungeon();
        dungeonNoCorridor.AddFirstRoom();
        dungeonNoCorridor.FillRoomToDoor();
        dungeonNoCorridor.AddEntryPoint();
        Draw(dungeonNoCorridor.DungeonTiles);
        var list = dungeonNoCorridor.DungeonTiles.SelectMany(T => T).ToList();
        var match = list.Where(x => x.Texture == Textures.Entry);
        match.ShouldNotBeNull();
    }

    [Fact]
    public void TestAddDescription()
    {
        using var env = new TestEnvironment();
        var dungeonNoCorridor = env.GetNcDungeon();
        dungeonNoCorridor.AddFirstRoom();
        dungeonNoCorridor.FillRoomToDoor();
        dungeonNoCorridor.AddEntryPoint();
        dungeonNoCorridor.AddDescription();
        Draw(dungeonNoCorridor.DungeonTiles);
        dungeonNoCorridor.RoomDescription.Count.ShouldBeGreaterThan(1);
    }
}