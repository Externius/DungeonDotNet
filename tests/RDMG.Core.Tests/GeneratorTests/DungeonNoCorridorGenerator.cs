using RDMG.Core.Domain;
using Shouldly;
using System.Linq;
using Xunit;

namespace RDMG.Core.Tests.GeneratorTests;

public class DungeonNoCorridorGenerator(ITestOutputHelper output) : DungeonTestBase(output)
{
    [Fact]
    public void CanInit()
    {
        Draw(DungeonNoCorridor.DungeonTiles);
        DungeonNoCorridor.RoomDescription.Count.ShouldBe(0);
    }

    [Fact]
    public void TestAddFirstRoom()
    {
        DungeonNoCorridor.AddFirstRoom();
        Draw(DungeonNoCorridor.DungeonTiles);
        var list = DungeonNoCorridor.DungeonTiles.SelectMany(T => T);
        var match = list.Where(x => x.Texture == Texture.Room);
        match.ShouldNotBeNull();
    }

    [Fact]
    public void TestFillRoomToDoor()
    {
        DungeonNoCorridor.AddFirstRoom();
        DungeonNoCorridor.FillRoomToDoor();
        Draw(DungeonNoCorridor.DungeonTiles);
        DungeonNoCorridor.OpenDoorList.Count.ShouldBe(0);
    }

    [Fact]
    public void TestAddEntryPoint()
    {
        DungeonNoCorridor.AddFirstRoom();
        DungeonNoCorridor.FillRoomToDoor();
        DungeonNoCorridor.AddEntryPoint();
        Draw(DungeonNoCorridor.DungeonTiles);
        var list = DungeonNoCorridor.DungeonTiles.SelectMany(T => T);
        var match = list.Where(x => x.Texture == Texture.Entry);
        match.ShouldNotBeNull();
    }

    [Fact]
    public void TestAddDescription()
    {
        DungeonNoCorridor.AddFirstRoom();
        DungeonNoCorridor.FillRoomToDoor();
        DungeonNoCorridor.AddEntryPoint();
        DungeonNoCorridor.AddDescription();
        Draw(DungeonNoCorridor.DungeonTiles);
        DungeonNoCorridor.RoomDescription.Count.ShouldBeGreaterThan(1);
    }
}