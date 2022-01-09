using AutoMapper;
using Microsoft.Extensions.Logging;
using RDMG.Core.Abstractions.Dungeon;
using RDMG.Core.Abstractions.Dungeon.Models;
using RDMG.Core.Abstractions.Services.Models;
using RDMG.Core.Domain;
using RDMG.Core.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace RDMG.Core.Generator
{
    public class DungeonHelper : IDungeonHelper
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        // encounter
        private List<Monster> FilteredMonsters;
        private int SumXP;
        private readonly int[] Difficulty = {
            0, 0, 0, 0
        };
        // trap
        private static Trap CurrentTrap;
        // treasure
        private int SumValue;
        // door
        private DungeonTile[][] DungeonTiles { get; set; }
        private List<DungeonTile> DoorList { get; set; }
        private int WestCount;
        private int SouthCount;
        private int EastCount;
        private int NorthCount;
        private static readonly Random _random = new();

        public DungeonHelper(IMapper mapper,
            ILogger<DungeonHelper> logger)
        {
            _mapper = mapper;
            _logger = logger;
        }

        public List<TreasureDescription> TreasureList { get; set; }
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
        internal int GetTreasurePercentage()
        {
            return DungeonDifficulty switch
            {
                0 => GetRandomInt(20, 71),
                1 => GetRandomInt(30, 81),
                2 => GetRandomInt(40, 91),
                3 => GetRandomInt(50, 101),
                _ => 0,
            };
        }

        internal int GetMonsterPercentage()
        {
            return DungeonDifficulty switch
            {
                0 => GetRandomInt(40, 81),
                1 => GetRandomInt(50, 91),
                2 => GetRandomInt(60, 101),
                3 => GetRandomInt(70, 101),
                _ => 0,
            };
        }

        public int GetRandomInt(int min, int max)
        {
            if (max != min)
                return _random.Next(max - min) + min;
            return max;
        }

        public void AddRoomDescription(DungeonTile[][] dungeonTiles, int x, int y, List<RoomDescription> roomDescription, List<DungeonTile> currentDoors)
        {

            dungeonTiles[x][y].Index = roomDescription.Count;
            DungeonTiles = dungeonTiles;
            DoorList = currentDoors;
            roomDescription.Add(new RoomDescription(
                GetRoomName(roomDescription.Count + 1),
                GetTreasure(),
                GetMonster(),
                GetDoorDescription()));
            dungeonTiles[x][y].Description = Convert.ToString(roomDescription.Count);
        }

        public void AddTrapDescription(DungeonTile[][] dungeonTiles, int x, int y, List<TrapDescription> trapDescription)
        {
            dungeonTiles[x][y].Index = trapDescription.Count;
            trapDescription.Add(new TrapDescription(
                GetTrapName(trapDescription.Count + 1),
                GetCurrentTrap(false)));
            dungeonTiles[x][y].Description = trapDescription.Count.ToString();
        }

        public void AddRoamingMonsterDescription(DungeonTile[][] dungeonTiles, int x, int y, List<RoamingMonsterDescription> roamingMonsterDescription)
        {

            dungeonTiles[x][y].Index = roamingMonsterDescription.Count;
            roamingMonsterDescription.Add(new RoamingMonsterDescription(
                GetRoamingName(roamingMonsterDescription.Count + 1),
                GetRoamingMonster()));
            dungeonTiles[x][y].Description = roamingMonsterDescription.Count.ToString();
        }

        public int Manhattan(int dx, int dy)
        {
            return dx + dy;
        }

        private static string GetRoomName(int x)
        {
            return "#ROOM" + x + "#";
        }

        public void AddNCRoomDescription(DungeonTile[][] dungeonTiles, int x, int y, List<RoomDescription> roomDescription, string doors)
        {

            dungeonTiles[x][y].Index = roomDescription.Count;
            roomDescription.Add(new RoomDescription(
                GetRoomName(roomDescription.Count + 1),
                GetTreasure(),
                GetMonster(),
                doors));
            dungeonTiles[x][y].Description = roomDescription.Count.ToString();
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

        public string GetMonster()
        {
            if (MonsterType.Equals("none", StringComparison.OrdinalIgnoreCase))
                return "Monster: None";
            EncounterInit();
            var checkResult = CheckPossible();
            if (checkResult && GetRandomInt(0, 101) <= GetMonsterPercentage())
                return CalcEncounter();
            else if (!checkResult)
                return "Monster: No suitable monsters with this settings";
            else
                return "Monster: None";
        }

        private static double Parse(string ratio)
        {
            if (ratio.Contains('/'))
            {
                var rat = ratio.Split("/");
                return double.Parse(rat[0]) / double.Parse(rat[1]);
            }
            else
            {
                return double.Parse(ratio);
            }
        }

        public void Init(DungeonOptionModel model)
        {
            PartyLevel = model.PartyLevel;
            PartySize = model.PartySize;
            TreasureValue = model.TreasureValue;
            ItemsRarity = model.ItemsRarity;
            DungeonDifficulty = model.DungeonDifficulty;
            MonsterType = model.MonsterType;
            MonsterList = DeseraliazerJSON<Monster>("5e-SRD-Monsters.json");
            TreasureList = DeseraliazerJSON<TreasureDescription>("treasures.json");
        }

        public static List<T> DeseraliazerJSON<T>(string fileName)
        {
            var json = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/Data/" + fileName);
            return JsonSerializer.Deserialize<List<T>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });
        }

        private static string GetRoamingName(int count)
        {
            return "ROAMING MONSTERS " + count + "# ";
        }

        public string GetRoamingMonster()
        {
            EncounterInit();
            if (CheckPossible())
                return CalcEncounter()[9..]; // remove "Monster: "
            else
                return "No suitable monsters with this settings";
        }

        private void EncounterInit()
        {
            Difficulty[0] = Constants.Thresholds[PartyLevel, 0] * PartySize;
            Difficulty[1] = Constants.Thresholds[PartyLevel, 1] * PartySize;
            Difficulty[2] = Constants.Thresholds[PartyLevel, 2] * PartySize;
            Difficulty[3] = Constants.Thresholds[PartyLevel, 3] * PartySize;
            FilteredMonsters = MonsterList;
            SumXP = Difficulty[DungeonDifficulty];
        }
        private bool CheckPossible()
        {
            foreach (var monster in FilteredMonsters)
            {
                if (SumXP > GetMonsterXP(monster))
                {
                    return true;
                }
            }
            return false;
        }

        private string CalcEncounter()
        {
            var result = new StringBuilder();
            result.Append("Monster: ");
            if (GetRandomInt(0, 101) > 50)
            {
                result.Append(AddMonster(SumXP));
            }
            else
            {
                var x = GetRandomInt(2, DungeonDifficulty + 3);
                for (var i = 0; i < x; i++)
                {
                    result.Append(AddMonster(SumXP / x));
                    result.Append(", ");
                }
                result.Length -= 2;
            }
            return result.Replace(", None", "").ToString();
        }

        private string AddMonster(int currentXP)
        {
            var monsterCount = FilteredMonsters.Count;
            var monster = 0;
            int count;
            double allXP;
            while (monster < monsterCount)
            {
                Monster currentMonster = FilteredMonsters[GetRandomInt(0, FilteredMonsters.Count)]; // get random monster
                FilteredMonsters.Remove(currentMonster);
                var monsterXP = GetMonsterXP(currentMonster);
                for (var i = Constants.Multipliers.GetLength(0) - 1; i > -1; i--)
                {
                    count = (int)Constants.Multipliers[i, 0];
                    allXP = monsterXP * count * Constants.Multipliers[i, 1];
                    if (allXP <= currentXP && count > 1)
                        return count + "x " + currentMonster.Name + " (CR: " + currentMonster.Challenge_Rating + ") " + monsterXP * count + " XP";
                    else if (allXP <= currentXP)
                        return currentMonster.Name + " (CR: " + currentMonster.Challenge_Rating + ") " + monsterXP + " XP";
                }
                monster++;
            }
            return "None";
        }
        private static int GetMonsterXP(Monster monster)
        {
            return Constants.ChallengeRatingXP[Constants.ChallengeRating.IndexOf(monster.Challenge_Rating)]; // get monster xp
        }

        public string GetCurrentTrap(bool door)
        {
            var trapSeverity = GetTrapSeverity();
            if (door) // get random trap
                CurrentTrap = Constants.DoorTraps[GetRandomInt(0, Constants.DoorTraps.Count)];
            else
                CurrentTrap = Constants.SimpleTraps[GetRandomInt(0, Constants.SimpleTraps.Count)];
            if (CurrentTrap.DmgType.HasValue)
                return CurrentTrap.Name + " [" + GetTrapSeverity() + "]: DC " + CurrentTrap.Spot + " to spot, DC " + CurrentTrap.Disable + " to disable (" + EnumHelper.GetDescription(CurrentTrap.DisableCheck) + "), DC " + GetTrapSaveDC((int)trapSeverity) + " " + CurrentTrap.Save + " save or take " + GetTrapDamage((int)trapSeverity) + "D10 (" + CurrentTrap.DmgType + ") damage" + GetTrapAttackBonus((int)trapSeverity);
            else
                return CurrentTrap.Name + " [" + GetTrapSeverity() + "]: DC " + CurrentTrap.Spot + " to spot, DC " + CurrentTrap.Disable + " to disable (" + EnumHelper.GetDescription(CurrentTrap.DisableCheck) + "), DC " + GetTrapSaveDC((int)trapSeverity) + " " + CurrentTrap.Save + " save or " + CurrentTrap.Special;
        }

        private string GetTrapAttackBonus(int trapDanger)
        {
            if (CurrentTrap.AttackMod)
            {
                var min = Constants.TrapAttackBonus[trapDanger];
                var max = Constants.TrapAttackBonus[trapDanger + 1];
                return ",\n (attack bonus +" + GetRandomInt(min, max) + ").";
            }
            return ".";
        }

        private int GetTrapDamage(int trapDanger)
        {
            if (PartyLevel < 5)
                return Constants.TrapDmgSeverity[0, trapDanger];
            else if (PartyLevel < 11)
                return Constants.TrapDmgSeverity[1, trapDanger];
            else if (PartyLevel < 17)
                return Constants.TrapDmgSeverity[2, trapDanger];
            else
                return Constants.TrapDmgSeverity[3, trapDanger];
        }

        private int GetTrapSaveDC(int trapDanger)
        {
            var min = Constants.TrapSave[trapDanger];
            var max = Constants.TrapSave[trapDanger + 1];
            return GetRandomInt(min, max);
        }

        private TrapSeverity GetTrapSeverity()
        {
            return DungeonDifficulty switch
            {
                0 => TrapSeverity.Setback,
                1 or 2 => TrapSeverity.Dangerous,
                3 => TrapSeverity.Deadly,
                _ => TrapSeverity.Setback,
            };
        }

        private static string GetTrapName(int count)
        {
            return "#TRAP" + count + "#";
        }

        public string GetTreasure()
        {
            if (GetRandomInt(0, 101) > GetTreasurePercentage())
            {
                return "Treasures: Empty";
            }
            GetAllCost();
            return "Treasures: " + CalcTreasure();
        }

        private string CalcTreasure()
        {
            var filteredTreasures = GetFilteredList();
            var sb = new StringBuilder();
            var currentValue = 0;
            var itemCount = GetItemsCount();
            var currentCount = 0;
            var maxAttempt = filteredTreasures.Count * 2;
            TreasureDescription currentTreasure;
            var finalList = new List<TreasureDescription>();
            while (currentCount < itemCount && maxAttempt > 0)
            {
                currentTreasure = filteredTreasures[GetRandomInt(0, filteredTreasures.Count)]; // get random treasure
                if (currentValue + currentTreasure.Cost < SumValue)
                { // if it's still affordable add to list
                    currentValue += currentTreasure.Cost;
                    finalList.Add(currentTreasure);
                    currentCount++;
                }
                maxAttempt--;
            }
            var query = finalList // get occurance
              .GroupBy(x => x)
              .ToDictionary(x => x.Key, y => y.Count());
            foreach (KeyValuePair<TreasureDescription, int> item in query)
            {
                if (item.Value > 1)
                {
                    sb.Append(item.Value);
                    sb.Append("x ");
                }
                sb.Append(item.Key.Name);
                sb.Append(", ");
            }
            sb.Append(GetRandomInt(1, SumValue - currentValue)); // get the remaining value randomly
            sb.Append(" gp");
            return sb.ToString();
        }

        private int GetItemsCount()
        {
            return DungeonDifficulty switch
            {
                0 => GetRandomInt(0, Constants.ItemCount[PartyLevel]),
                1 => GetRandomInt(2, Constants.ItemCount[PartyLevel]),
                2 => GetRandomInt(3, Constants.ItemCount[PartyLevel]),
                3 => GetRandomInt(4, Constants.ItemCount[PartyLevel]),
                _ => 0,
            };
        }

        private List<TreasureDescription> GetFilteredList()
        {
            if (MonsterType.Equals("any", StringComparison.OrdinalIgnoreCase))
            {
                return TreasureList
                .Where(item => item.Rarity <= ItemsRarity && item.Cost < SumValue)
                .ToList();
            }
            else
            {
                return TreasureList
                .Where(item => item.Rarity <= ItemsRarity && item.Cost < SumValue && item.Types.Contains(MonsterType))
                .ToList();
            }
        }

        private void GetAllCost()
        {
            SumValue = (int)(Constants.TreasureGP[PartyLevel] * TreasureValue);
        }

        private string GetDoorDescription()
        {
            var sb = new StringBuilder();
            int start;
            int end;
            WestCount = 1;
            SouthCount = 1;
            EastCount = 1;
            NorthCount = 1;
            foreach (var door in DoorList)
            {
                start = sb.Length;
                RoomPosition.CheckRoomPosition(DungeonTiles, door.I, door.J);
                sb.Append(GetDoorText(door.Texture, GetDoorDC()));
                end = sb.Length;
                DungeonTiles[door.I][door.J].Description = sb.ToString(start, end - start);
            }
            sb.Length -= 1;
            return sb.ToString();
        }

        private string GetDoorText(Textures texture, int x)
        {
            if (RoomPosition.Up)
                return "South Entry #" + SouthCount++ + ": " + Constants.DoorTypes[x] + GetState(texture, x) + "\n";
            else if (RoomPosition.Down)
                return "North Entry #" + NorthCount++ + ": " + Constants.DoorTypes[x] + GetState(texture, x) + "\n";
            else if (RoomPosition.Right)
                return "West Entry #" + WestCount++ + ": " + Constants.DoorTypes[x] + GetState(texture, x) + "\n";
            else
                return "East Entry #" + EastCount++ + ": " + Constants.DoorTypes[x] + GetState(texture, x) + "\n";
        }

        private string GetState(Textures texture, int x)
        {
            return texture switch
            {
                Textures.NO_CORRIDOR_DOOR_LOCKED or Textures.DOOR_LOCKED => " Locked Door (AC " + Constants.DoorAC[x] + ", HP " + Constants.DoorHP[x] + ", DC " + Constants.LockDifficulty[x] + " to unlock)",
                Textures.NO_CORRIDOR_DOOR_TRAPPED or Textures.DOOR_TRAPPED => " Trapped Door (AC " + Constants.DoorAC[x] + ", HP " + Constants.DoorHP[x] + ") " + GetCurrentTrap(true),
                _ => " Open Door (AC " + Constants.DoorAC[x] + ", HP " + Constants.DoorHP[x] + ")",
            };
        }

        private int GetDoorDC()
        {
            return DungeonDifficulty switch
            {
                0 => GetRandomInt(0, 2),
                1 => GetRandomInt(1, 4),
                2 => GetRandomInt(1, 6),
                3 => GetRandomInt(2, 7),
                _ => 0,
            };
        }

        public bool CheckNCDoor(DungeonTile[][] dungeonTiles, int x, int y)
        {
            return dungeonTiles[x][y].Texture == Textures.NO_CORRIDOR_DOOR || dungeonTiles[x][y].Texture == Textures.NO_CORRIDOR_DOOR_LOCKED || dungeonTiles[x][y].Texture == Textures.NO_CORRIDOR_DOOR_TRAPPED;
        }

        public string GetNCDoorDescription(DungeonTile[][] dungeonTiles, List<DungeonTile> closedList)
        {
            WestCount = 1;
            SouthCount = 1;
            EastCount = 1;
            NorthCount = 1;
            var sb = new StringBuilder();
            foreach (var tile in closedList)
            {
                if (CheckNCDoor(dungeonTiles, tile.I, tile.J - 1))
                {
                    sb.Append("West Entry #");
                    sb.Append(WestCount++);
                    sb.Append(dungeonTiles[tile.I][tile.J - 1].Description);
                }
                else if (CheckNCDoor(dungeonTiles, tile.I, tile.J + 1))
                {
                    sb.Append("East Entry #");
                    sb.Append(EastCount++);
                    sb.Append(dungeonTiles[tile.I][tile.J + 1].Description);
                }
                else if (CheckNCDoor(dungeonTiles, tile.I + 1, tile.J))
                {
                    sb.Append("South Entry #");
                    sb.Append(SouthCount++);
                    sb.Append(dungeonTiles[tile.I + 1][tile.J].Description);
                }
                else if (CheckNCDoor(dungeonTiles, tile.I - 1, tile.J))
                {
                    sb.Append("North Entry #");
                    sb.Append(NorthCount++);
                    sb.Append(dungeonTiles[tile.I - 1][tile.J].Description);
                }
            }
            sb.Length -= 1;
            return sb.ToString();
        }

        public string GetNCDoor(DungeonTile door)
        {
            var doorDC = GetDoorDC();
            return ": " + Constants.DoorTypes[doorDC] + GetState(door.Texture, doorDC) + "\n";
        }
    }
}