using RDMG.Core.Domain;
using Shouldly;
using System.Linq;
using Xunit;

namespace RDMG.Tests.GeneratorTests
{
    public class DungeonGenerator
    {
        [Fact]
        public void CanInit()
        {
            using var env = new TestEnvironment();
            var dungeon = env.GetDungeon();
            dungeon.Init();
            DrawTestDungeon.Draw(dungeon.DungeonTiles);
            dungeon.RoomDescription.Count.ShouldBe(0);
        }

        [Fact]
        public void CanGenerateRoom()
        {
            using var env = new TestEnvironment();
            var dungeon = env.GetDungeon();
            dungeon.Init();
            dungeon.GenerateRoom();
            DrawTestDungeon.Draw(dungeon.DungeonTiles);
            dungeon.RoomDescription.Count.ShouldBe(2);
        }

        [Fact]
        public void CanAddEntryPoint()
        {
            using var env = new TestEnvironment();
            var dungeon = env.GetDungeon();
            dungeon.Init();
            dungeon.GenerateRoom();
            dungeon.AddEntryPoint();
            DrawTestDungeon.Draw(dungeon.DungeonTiles);
            var list = dungeon.DungeonTiles.SelectMany(T => T).ToList();
            var match = list.Where(x => x.Texture == Textures.ENTRY);
            match.ShouldNotBeNull();
        }
        [Fact]
        public void TestGenerateCorridors()
        {
            using var env = new TestEnvironment();
            var dungeon = env.GetDungeon();
            dungeon.Init();
            dungeon.GenerateRoom();
            dungeon.AddEntryPoint();
            dungeon.GenerateCorridors();
            DrawTestDungeon.Draw(dungeon.DungeonTiles);
            var list = dungeon.DungeonTiles.SelectMany(T => T).ToList();
            var match = list.Where(x => x.Texture == Textures.CORRIDOR);
            match.ShouldNotBeNull();
        }
        [Fact]
        public void TestAddDeadEnds()
        {
            using var env = new TestEnvironment();
            var dungeon = env.GetDungeon();
            dungeon.Init();
            dungeon.GenerateRoom();
            dungeon.AddEntryPoint();
            dungeon.GenerateCorridors();
            dungeon.AddDeadEnds();
            DrawTestDungeon.Draw(dungeon.DungeonTiles);
            var list = dungeon.DungeonTiles.SelectMany(T => T).ToList();
            var match = list.Where(x => x.Texture == Textures.CORRIDOR);
            match.ShouldNotBeNull();
        }
        [Fact]
        public void TestAddTrap()
        {
            using var env = new TestEnvironment();
            var dungeon = env.GetDungeon();
            dungeon.Init();
            dungeon.GenerateRoom();
            dungeon.AddEntryPoint();
            dungeon.GenerateCorridors();
            dungeon.AddDeadEnds();
            dungeon.AddCorridorItem(2, Item.TRAP);
            DrawTestDungeon.Draw(dungeon.DungeonTiles);
            var list = dungeon.DungeonTiles.SelectMany(T => T).ToList();
            var match = list.Where(x => x.Texture == Textures.TRAP);
            match.ShouldNotBeNull();
        }
        [Fact]
        public void TestAddRoamingMonster()
        {
            using var env = new TestEnvironment();
            var dungeon = env.GetDungeon();
            dungeon.Init();
            dungeon.GenerateRoom();
            dungeon.AddEntryPoint();
            dungeon.GenerateCorridors();
            dungeon.AddDeadEnds();
            dungeon.AddCorridorItem(2, Item.ROAMING_MONSTER);
            DrawTestDungeon.Draw(dungeon.DungeonTiles);
            var list = dungeon.DungeonTiles.SelectMany(T => T).ToList();
            var match = list.Where(x => x.Texture == Textures.ROAMING_MONSTER);
            match.ShouldNotBeNull();
        }
    }
}
