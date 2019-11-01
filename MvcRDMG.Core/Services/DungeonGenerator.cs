using MvcRDMG.Core.Abstractions.Services;
using MvcRDMG.Core.Abstractions.Services.Models;
using MvcRDMG.Generator.Core;
using MvcRDMG.Generator.Helpers;
using MvcRDMG.Generator.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace MvcRDMG.Core.Services
{
    public class DungeonGenerator : IDungeonGenerator
    {
        private static readonly int dungeonWidthAndHeight = 800;
        public bool Generate(OptionModel model)
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
                    var dungeon = new Dungeon(dungeonWidthAndHeight, dungeonWidthAndHeight, model.DungeonSize, model.RoomDensity, model.RoomSize, model.TrapPercent, model.DeadEnd, model.RoamingPercent);
                    dungeon.Generate();
                    model.SavedDungeons = new List<SavedDungeonModel>()
                    {
                        new SavedDungeonModel()
                        {
                            DungeonTiles = JsonSerializer.Serialize(dungeon.DungeonTiles),
                            RoomDescription = JsonSerializer.Serialize(dungeon.RoomDescription),
                            TrapDescription = JsonSerializer.Serialize(dungeon.TrapDescription),
                            RoamingMonsterDescription = JsonSerializer.Serialize(dungeon.RoamingMonsterDescription)
                        }
                    };
                    return true;
                }
                else
                {
                    var dungeonNoCorridor = new DungeonNoCorridor(dungeonWidthAndHeight, dungeonWidthAndHeight, model.DungeonSize, model.RoomSize);
                    dungeonNoCorridor.Generate();
                    model.SavedDungeons = new List<SavedDungeonModel>()
                    {
                        new SavedDungeonModel()
                        {
                            DungeonTiles = JsonSerializer.Serialize(dungeonNoCorridor.DungeonTiles),
                            RoomDescription = JsonSerializer.Serialize(dungeonNoCorridor.RoomDescription)
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
            string json = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/Data/" + fileName);
            return JsonSerializer.Deserialize<List<T>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });
        }
    }
}