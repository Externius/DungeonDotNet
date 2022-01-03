using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDMG.Core.Abstractions.Dungeon.Models;
using RDMG.Core.Domain;
using RDMG.Core.Generator;
using RDMG.Core.Helpers;
using System.Linq;

namespace RDMG.Tests
{
    [TestClass]
    public class DungeonNoCorridorTest
    {
        private readonly DungeonNoCorridor dungeonNoCorridor = new(800, 800, 15, 15);

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
            dungeonNoCorridor.Init();
        }
        [TestMethod]
        public void TestInit()
        {
            DrawTestDungeon.Draw(dungeonNoCorridor.DungeonTiles);
            Assert.IsTrue(dungeonNoCorridor.RoomDescription.Count == 0);
        }
        [TestMethod]
        public void TestAddFirstRoom()
        {
            dungeonNoCorridor.AddFirstRoom();
            DrawTestDungeon.Draw(dungeonNoCorridor.DungeonTiles);
            var list = dungeonNoCorridor.DungeonTiles.SelectMany(T => T).ToList();
            var match = list.Where(x => x.Texture == Textures.ROOM);
            Assert.IsTrue(match != null);
        }

        [TestMethod]
        public void TestFillRoomToDoor()
        {
            dungeonNoCorridor.AddFirstRoom();
            dungeonNoCorridor.FillRoomToDoor();
            DrawTestDungeon.Draw(dungeonNoCorridor.DungeonTiles);
            Assert.IsTrue(dungeonNoCorridor.OpenDoorList.Count == 0);
        }

        [TestMethod]
        public void TestAddEntryPoint()
        {
            dungeonNoCorridor.AddFirstRoom();
            dungeonNoCorridor.FillRoomToDoor();
            dungeonNoCorridor.AddEntryPoint();
            DrawTestDungeon.Draw(dungeonNoCorridor.DungeonTiles);
            var list = dungeonNoCorridor.DungeonTiles.SelectMany(T => T).ToList();
            var match = list.Where(x => x.Texture == Textures.ENTRY);
            Assert.IsTrue(match != null);
        }

        [TestMethod]
        public void TestAddDescription()
        {
            dungeonNoCorridor.AddFirstRoom();
            dungeonNoCorridor.FillRoomToDoor();
            dungeonNoCorridor.AddEntryPoint();
            dungeonNoCorridor.AddDescription();
            DrawTestDungeon.Draw(dungeonNoCorridor.DungeonTiles);
            Assert.IsTrue(dungeonNoCorridor.RoomDescription.Count > 1);
        }
    }
}