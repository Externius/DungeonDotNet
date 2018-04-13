using System;
using System.Collections.Generic;
using System.Linq;
using MvcRDMG.Generator.Models;

namespace MvcRDMG.Generator.Helpers
{
    public class Utils
    {
        private static readonly Random _random = new Random();
        private static readonly Utils _instance = new Utils();
        public static Utils Instance { get { return _instance; } }
        public IDoor DoorGenerator { get; set; }
        public ITrap TrapGenerator { get; set; }
        public ITreasure TreasureGenerator { get; set; }
        public IEncounter EncouterGenerator { get; set; }
        public List<Treasures> TreasureList { get; set; }
        private List<Monster> _monsterList;
        public List<Monster> MonsterList
        {
            get { return _monsterList; }
            set
            {
                _monsterList = GetMonsters(value);  // get monsters for party level
            }
        }
        public int PartyLevel { get; set; }
        public int PartySize { get; set; }
        public int DungeonDifficulty { get; set; }
        public string MonsterType { get; set; }
        public double TreasureValue { get; set; }
        public int ItemsRarity { get; set; }
        private Utils()
        {

        }
        internal int GetTreasurePercentage()
        {
            switch (DungeonDifficulty)
            {
                case 0:
                    return GetRandomInt(20, 71);
                case 1:
                    return GetRandomInt(30, 81);
                case 2:
                    return GetRandomInt(40, 91);
                case 3:
                    return GetRandomInt(50, 101);
                default:
                    return 0;
            }
        }
        internal int GetMonsterPercentage()
        {
            switch (DungeonDifficulty)
            {
                case 0:
                    return GetRandomInt(40, 81);
                case 1:
                    return GetRandomInt(50, 91);
                case 2:
                    return GetRandomInt(60, 101);
                case 3:
                    return GetRandomInt(70, 101);
                default:
                    return 0;
            }
        }
        public int GetRandomInt(int min, int max)
        {
            if (max != min)
            {
                return _random.Next(max - min) + min;
            }
            return max;
        }
        public void AddRoomDescription(DungeonTile[][] dungeonTiles, int x, int y, List<RoomDescription> roomDescription, List<DungeonTile> currentDoors)
        {
            if (DoorGenerator != null && TreasureGenerator != null && EncouterGenerator != null)
            {
                dungeonTiles[x][y].Index = roomDescription.Count;
                DoorGenerator.DungeonTiles = dungeonTiles;
                DoorGenerator.DoorList = currentDoors;
                roomDescription.Add(new RoomDescription(
                    GetRoomName(roomDescription.Count + 1),
                    TreasureGenerator.GetTreasure(),
                    EncouterGenerator.GetMonster(),
                    DoorGenerator.GetDoorDescription()));
                dungeonTiles[x][y].Description = Convert.ToString(roomDescription.Count);
            }
        }
        public void AddTrapDescription(DungeonTile[][] dungeonTiles, int x, int y, List<TrapDescription> trapDescription)
        {
            if (TrapGenerator != null)
            {
                dungeonTiles[x][y].Index = trapDescription.Count;
                trapDescription.Add(new TrapDescription(
                    TrapGenerator.GetTrapName(trapDescription.Count + 1),
                    TrapGenerator.GetCurrentTrap(false)));
                dungeonTiles[x][y].Description = trapDescription.Count.ToString();
            }
        }
        public int Manhattan(int dx, int dy)
        {
            return dx + dy;
        }
        private string GetRoomName(int x)
        {
            return "#ROOM" + x + "#";
        }
        public void AddNCRoomDescription(DungeonTile[][] dungeonTiles, int x, int y, List<RoomDescription> roomDescription, string doors)
        {
            if (TreasureGenerator != null && EncouterGenerator != null)
            {
                dungeonTiles[x][y].Index = roomDescription.Count;
                roomDescription.Add(new RoomDescription(
                    GetRoomName(roomDescription.Count + 1),
                    TreasureGenerator.GetTreasure(),
                    EncouterGenerator.GetMonster(),
                    doors));
                dungeonTiles[x][y].Description = roomDescription.Count.ToString();
            }
        }
        private List<Monster> GetMonsters(List<Monster> monsters)
        {
            if (MonsterType.Equals("any", StringComparison.OrdinalIgnoreCase))
            {
                return monsters.Where(monster => Parse(monster.Challenge_Rating) <= PartyLevel + 2 &&
                    Parse(monster.Challenge_Rating) >= PartyLevel / 4)
                    .ToList();
            }
            else
            {
                return monsters.Where(monster => Parse(monster.Challenge_Rating) <= PartyLevel + 2 &&
                    Parse(monster.Challenge_Rating) >= PartyLevel / 4 &&
                    MonsterType.Contains(monster.Type))
                    .ToList();
            }
        }
        private double Parse(string ratio)
        {
            if (ratio.Contains("/"))
            {
                String[] rat = ratio.Split("/");
                return Double.Parse(rat[0]) / Double.Parse(rat[1]);
            }
            else
            {
                return Double.Parse(ratio);
            }
        }
    }
}