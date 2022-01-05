using RDMG.Core.Domain;
using Shouldly;
using System.Linq;
using Xunit;

namespace RDMG.Tests.GeneratorTests
{
    public class DungeonNoCorridorGenerator
    {
        [Fact]
        public void CanInit()
        {
            using var env = new TestEnvironment();
            var dungeonNoCorridor = env.GetNCDungeon();
            dungeonNoCorridor.Init();
            DrawTestDungeon.Draw(dungeonNoCorridor.DungeonTiles);
            dungeonNoCorridor.RoomDescription.Count.ShouldBe(0);
        }

        [Fact]
        public void TestAddFirstRoom()
        {
            using var env = new TestEnvironment();
            var dungeonNoCorridor = env.GetNCDungeon();
            dungeonNoCorridor.Init();
            dungeonNoCorridor.AddFirstRoom();
            DrawTestDungeon.Draw(dungeonNoCorridor.DungeonTiles);
            var list = dungeonNoCorridor.DungeonTiles.SelectMany(T => T).ToList();
            var match = list.Where(x => x.Texture == Textures.ROOM);
            match.ShouldNotBeNull();
        }

        [Fact]
        public void TestFillRoomToDoor()
        {
            using var env = new TestEnvironment();
            var dungeonNoCorridor = env.GetNCDungeon();
            dungeonNoCorridor.Init();
            dungeonNoCorridor.AddFirstRoom();
            dungeonNoCorridor.FillRoomToDoor();
            DrawTestDungeon.Draw(dungeonNoCorridor.DungeonTiles);
            dungeonNoCorridor.OpenDoorList.Count.ShouldBe(0);
        }

        [Fact]
        public void TestAddEntryPoint()
        {
            using var env = new TestEnvironment();
            var dungeonNoCorridor = env.GetNCDungeon();
            dungeonNoCorridor.Init();
            dungeonNoCorridor.AddFirstRoom();
            dungeonNoCorridor.FillRoomToDoor();
            dungeonNoCorridor.AddEntryPoint();
            DrawTestDungeon.Draw(dungeonNoCorridor.DungeonTiles);
            var list = dungeonNoCorridor.DungeonTiles.SelectMany(T => T).ToList();
            var match = list.Where(x => x.Texture == Textures.ENTRY);
            match.ShouldNotBeNull();
        }

        [Fact]
        public void TestAddDescription()
        {
            using var env = new TestEnvironment();
            var dungeonNoCorridor = env.GetNCDungeon();
            dungeonNoCorridor.Init();
            dungeonNoCorridor.AddFirstRoom();
            dungeonNoCorridor.FillRoomToDoor();
            dungeonNoCorridor.AddEntryPoint();
            dungeonNoCorridor.AddDescription();
            DrawTestDungeon.Draw(dungeonNoCorridor.DungeonTiles);
            dungeonNoCorridor.RoomDescription.Count.ShouldBeGreaterThan(1);
        }
    }
}
