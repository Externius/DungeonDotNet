using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using MvcRDMG.Generator.Core;
using MvcRDMG.Generator.Helpers;
using MvcRDMG.Generator.Models;
using MvcRDMG.Models;
using MvcRDMG.ViewModels;
using Newtonsoft.Json;

namespace MvcRDMG.Services
{
    public class DungeonGenerator : IDungeonGenerator
    {
        public bool Generate(OptionViewModel model)
        {
            Utils.Instance.DoorGenerator = new Door();
            Utils.Instance.TrapGenerator = new Trap();
            Utils.Instance.TreasureGenerator = new Treasure();
            Utils.Instance.EncouterGenerator = new Encounter();
            Utils.Instance.PartyLevel = model.PartyLevel;
            Utils.Instance.PartySize = model.PartySize;
            Utils.Instance.TreasureValue = model.TreasureValue;
            Utils.Instance.ItemsRarity = model.ItemsRarity;
            Utils.Instance.DungeonDifficulty = model.DungeonDifficulty;
            Utils.Instance.MonsterType = model.MonsterType;
            Utils.Instance.MonsterList = DeseraliazerJSON<Monster>("5e-SRD-Monsters.json");
            Utils.Instance.TreasureList = DeseraliazerJSON<Treasures>("treasures.json");
            try
            {
                if (model.Corridor)
                {
                    var dungeon = new Dungeon(800, 800, model.DungeonSize, model.RoomDensity, model.RoomSize, model.TrapPercent, model.DeadEnd, model.RoamingPercent);
                    dungeon.Generate();
                    model.SavedDungeons = new List<SavedDungeon>()
                    {
                        new SavedDungeon()
                        {
                            DungeonTiles = JsonConvert.SerializeObject(dungeon.DungeonTiles),
                            RoomDescription = JsonConvert.SerializeObject(dungeon.RoomDescription),
                            TrapDescription = JsonConvert.SerializeObject(dungeon.TrapDescription),
                            RoamingMonsterDescription = JsonConvert.SerializeObject(dungeon.RoamingMonsterDescription)
                }
                    };
                    return true;
                }
                else
                {
                    var dungeonNoCorridor = new DungeonNoCorridor(800, 800, model.DungeonSize, model.RoomSize);
                    dungeonNoCorridor.Generate();
                    model.SavedDungeons = new List<SavedDungeon>()
                    {
                        new SavedDungeon()
                        {
                            DungeonTiles = JsonConvert.SerializeObject(dungeonNoCorridor.DungeonTiles),
                            RoomDescription = JsonConvert.SerializeObject(dungeonNoCorridor.RoomDescription)
                        }
                    };
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                return false;
            }
        }
        public List<T> DeseraliazerJSON<T>(string fileName)
        {
            string json = File.ReadAllText("Generator/Data/" + fileName);
            var jsonList = JsonConvert.DeserializeObject<List<T>>(json);
            return jsonList;
        }
    }
}