using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcRDMG.Generator.Core;
using MvcRDMG.Generator.Helpers;
using MvcRDMG.Generator.Models;
using System.Linq;

namespace MvcRDMG.Tests
{
    [TestClass]
    public class DungeonTest
    {
        private readonly Dungeon dungeon = new(800, 800, 15, 10, 15, 15, true, 10);

        [TestInitialize]
        public void Setup()
        {
            Utils.Instance.DoorGenerator = new Door();
            Utils.Instance.TrapGenerator = new Trap();
            Utils.Instance.TreasureGenerator = new Treasure();
            Utils.Instance.EncouterGenerator = new Encounter();
            Utils.Instance.PartyLevel = 1;
            Utils.Instance.PartySize = 4;
            Utils.Instance.TreasureValue = 1;
            Utils.Instance.ItemsRarity = 1;
            Utils.Instance.DungeonDifficulty = 1;
            Utils.Instance.MonsterType = "any";
            Utils.Instance.MonsterList = Helpers.DeseraliazerJSON<Monster>("5e-SRD-Monsters.json");
            Utils.Instance.TreasureList = Helpers.DeseraliazerJSON<Treasures>("treasures.json");
            dungeon.Init();
        }

        [TestMethod]
        public void TestInit()
        {
            DrawTestDungeon.Draw(dungeon.DungeonTiles);
            Assert.IsTrue(dungeon.RoomDescription.Count == 0);
        }

        [TestMethod]
        public void TestGenerateRoom()
        {
            dungeon.GenerateRoom();
            DrawTestDungeon.Draw(dungeon.DungeonTiles);
            Assert.IsTrue(dungeon.RoomDescription.Count == 2);
        }

        [TestMethod]
        public void TestAddEntryPoint()
        {
            dungeon.GenerateRoom();
            dungeon.AddEntryPoint();
            DrawTestDungeon.Draw(dungeon.DungeonTiles);
            var list = dungeon.DungeonTiles.SelectMany(T => T).ToList();
            var match = list.Where(x => x.Texture == Textures.ENTRY);
            Assert.IsTrue(match != null);
        }
        [TestMethod]
        public void TestGenerateCorridors()
        {
            dungeon.GenerateRoom();
            dungeon.AddEntryPoint();
            dungeon.GenerateCorridors();
            DrawTestDungeon.Draw(dungeon.DungeonTiles);
            var list = dungeon.DungeonTiles.SelectMany(T => T).ToList();
            var match = list.Where(x => x.Texture == Textures.CORRIDOR);
            Assert.IsTrue(match != null);
        }
        [TestMethod]
        public void TestAddDeadEnds()
        {
            dungeon.GenerateRoom();
            dungeon.AddEntryPoint();
            dungeon.GenerateCorridors();
            dungeon.AddDeadEnds();
            DrawTestDungeon.Draw(dungeon.DungeonTiles);
            var list = dungeon.DungeonTiles.SelectMany(T => T).ToList();
            var match = list.Where(x => x.Texture == Textures.CORRIDOR);
            Assert.IsTrue(match != null);
        }
        [TestMethod]
        public void TestAddTrap()
        {
            dungeon.GenerateRoom();
            dungeon.AddEntryPoint();
            dungeon.GenerateCorridors();
            dungeon.AddDeadEnds();
            dungeon.AddCorridorItem(2, Item.TRAP);
            DrawTestDungeon.Draw(dungeon.DungeonTiles);
            var list = dungeon.DungeonTiles.SelectMany(T => T).ToList();
            var match = list.Where(x => x.Texture == Textures.TRAP);
            Assert.IsTrue(match != null);
        }
        [TestMethod]
        public void TestAddRoamingMonster()
        {
            dungeon.GenerateRoom();
            dungeon.AddEntryPoint();
            dungeon.GenerateCorridors();
            dungeon.AddDeadEnds();
            dungeon.AddCorridorItem(2, Item.ROAMING_MONSTER);
            DrawTestDungeon.Draw(dungeon.DungeonTiles);
            var list = dungeon.DungeonTiles.SelectMany(T => T).ToList();
            var match = list.Where(x => x.Texture == Textures.ROAMING_MONSTER);
            Assert.IsTrue(match != null);
        }
    }
}