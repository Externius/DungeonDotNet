using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDMG.Core.Abstractions.Dungeon.Models;
using RDMG.Core.Domain;
using RDMG.Core.Generator;
using RDMG.Core.Helpers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RDMG.Tests
{
    [TestClass]
    public class GeneratorsTest
    {
        private readonly Dungeon dungeon = new(800, 800, 15, 10, 15, 15, true, 10);

        [TestInitialize]
        public void Setup()
        {
            Utils.Instance.DoorGenerator = new Door();
            Utils.Instance.TrapGenerator = new Trap();
            Utils.Instance.TreasureGenerator = new Treasure();
            Utils.Instance.EncouterGenerator = new Encounter();
            Utils.Instance.PartyLevel = 3;
            Utils.Instance.PartySize = 4;
            Utils.Instance.TreasureValue = 2;
            Utils.Instance.ItemsRarity = 2;
            Utils.Instance.DungeonDifficulty = 1;
            Utils.Instance.MonsterType = "any";
            Utils.Instance.MonsterList = Helpers.DeseraliazerJSON<Monster>("5e-SRD-Monsters.json");
            Utils.Instance.TreasureList = Helpers.DeseraliazerJSON<Treasures>("treasures.json");
            dungeon.Init();
        }

        [TestMethod]
        public void TestGetDoorDescription()
        {
            var currentDoors = new List<DungeonTile>(){
                    new DungeonTile(2,2,2,2,50,50,Textures.DOOR),
                    new DungeonTile(5,5,2,2,50,50,Textures.DOOR_TRAPPED),
                    new DungeonTile(5,2,2,2,50,50,Textures.DOOR_LOCKED)
                };
            Utils.Instance.DoorGenerator.DungeonTiles = dungeon.DungeonTiles;
            Utils.Instance.DoorGenerator.DoorList = currentDoors;
            var result = Utils.Instance.DoorGenerator.GetDoorDescription();
            Trace.WriteLine("Result: " + result);
            Assert.IsTrue(result != null);
        }

        [TestMethod]
        public void TestGetNCDoor()
        {
            var currentDoors = new List<DungeonTile>(){
                    new DungeonTile(2,2,2,2,50,50,Textures.NO_CORRIDOR_DOOR),
                    new DungeonTile(5,5,2,2,50,50,Textures.NO_CORRIDOR_DOOR_TRAPPED),
                    new DungeonTile(5,2,2,2,50,50,Textures.NO_CORRIDOR_DOOR_LOCKED)
                };
            string[] result = new string[currentDoors.Count];
            for (int i = 0; i < currentDoors.Count; i++)
            {
                result[i] = Utils.Instance.DoorGenerator.GetNCDoor(currentDoors[i]);
                Trace.WriteLine("Door#" + i + " " + result[i]);
            }

            Assert.IsTrue(result[0] != null);
        }

        [TestMethod]
        public void TestGetNCDoorDescription()
        {
            var dungeonNoCorridor = new DungeonNoCorridor(800, 800, 15, 15);
            dungeonNoCorridor.Init();
            dungeonNoCorridor.AddFirstRoom();
            var list = dungeonNoCorridor.DungeonTiles.SelectMany(T => T).ToList();
            var match = list.Where(x => x.Texture == Textures.ROOM);
            var result = Utils.Instance.DoorGenerator.GetNCDoorDescription(dungeonNoCorridor.DungeonTiles, match.ToList());
            Assert.IsTrue(result != null);
        }
        [TestMethod]
        public void TestGetTreasure()
        {
            var result = new string[20];
            for (var i = 0; i < 20; i++)
            {
                result[i] = Utils.Instance.TreasureGenerator.GetTreasure();
                Trace.WriteLine("Treasure#" + i + " " + result[i]);
            }
            Assert.IsTrue(result[0] != null);
        }

        [TestMethod]
        public void TestGetMonster()
        {
            var result = new string[20];
            for (var i = 0; i < 20; i++)
            {
                result[i] = Utils.Instance.EncouterGenerator.GetMonster();
                Trace.WriteLine("Monster#" + i + " " + result[i]);
            }
            Assert.IsTrue(result[0] != null);
        }
        [TestMethod]
        public void TestGetCurrentTrap()
        {
            var result = new string[20];
            for (var i = 0; i < 20; i++)
            {
                result[i] = Utils.Instance.TrapGenerator.GetCurrentTrap(false);
                Trace.WriteLine("Trap#" + i + " " + result[i]);
            }
            Assert.IsTrue(result[0] != null);

        }
        [TestMethod]
        public void TestMonsterType()
        {
            Utils.Instance.MonsterType = "NonE";
            var result = Utils.Instance.EncouterGenerator.GetMonster();
            Trace.WriteLine(result);
            Assert.IsTrue(result.Equals("Monster: None"));
        }
        [TestMethod]
        public void TestGetRoamingMonster()
        {
            var result = new string[20];
            for (var i = 0; i < 20; i++)
            {
                result[i] = Utils.Instance.EncouterGenerator.GetRoamingMonster();
                Trace.WriteLine("Roaming Monster#" + i + " " + result[i]);
            }
            Assert.IsTrue(result[0] != null);
        }
    }
}