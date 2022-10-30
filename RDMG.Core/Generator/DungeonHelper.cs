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

namespace RDMG.Core.Generator;

public class DungeonHelper : IDungeonHelper
{
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    // encounter
    private List<Monster> FilteredMonsters;
    private int SumXp;
    private readonly int[] Difficulty = {
        0, 0, 0, 0
    };
    // trap
    private static Trap _currentTrap;
    // treasure
    private int SumValue;
    // door
    private DungeonTile[][] DungeonTiles { get; set; }
    private List<DungeonTile> DoorList { get; set; }
    private int WestCount;
    private int SouthCount;
    private int EastCount;
    private int NorthCount;
    private static readonly Random Random = new();

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
        get => _monsterList;
        set => _monsterList = GetMonsters(value); // get monsters for party level
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
            return Random.Next(max - min) + min;
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

    public void AddNcRoomDescription(DungeonTile[][] dungeonTiles, int x, int y, List<RoomDescription> roomDescription, string doors)
    {

        dungeonTiles[x][y].Index = roomDescription.Count;
        roomDescription.Add(new RoomDescription(
            GetRoomName(roomDescription.Count + 1),
            GetTreasure(),
            GetMonster(),
            doors));
        dungeonTiles[x][y].Description = roomDescription.Count.ToString();
    }

    private List<Monster> GetMonsters(IEnumerable<Monster> monsters)
    {
        if (MonsterType.Equals("any", StringComparison.OrdinalIgnoreCase))
        {
            return monsters.Where(monster => Parse(monster.Challenge_Rating) <= PartyLevel + 2 &&
                                             Parse(monster.Challenge_Rating) >= PartyLevel / 4.0)
                .ToList();
        }

        return monsters.Where(monster => Parse(monster.Challenge_Rating) <= PartyLevel + 2 &&
                                         Parse(monster.Challenge_Rating) >= PartyLevel / 4.0 &&
                                         MonsterType.Contains(monster.Type))
            .ToList();
    }

    public string GetMonster()
    {
        if (MonsterType.Equals("none", StringComparison.OrdinalIgnoreCase))
            return "Monster: None";
        EncounterInit();
        var checkResult = CheckPossible();
        if (checkResult && GetRandomInt(0, 101) <= GetMonsterPercentage())
            return CalcEncounter();
        if (!checkResult)
            return "Monster: No suitable monsters with this settings";
        return "Monster: None";
    }

    private static double Parse(string ratio)
    {
        if (!ratio.Contains('/'))
            return double.Parse(ratio);

        var rat = ratio.Split("/");
        return double.Parse(rat[0]) / double.Parse(rat[1]);
    }

    public void Init(DungeonOptionModel model)
    {
        PartyLevel = model.PartyLevel;
        PartySize = model.PartySize;
        TreasureValue = model.TreasureValue;
        ItemsRarity = model.ItemsRarity;
        DungeonDifficulty = model.DungeonDifficulty;
        MonsterType = model.MonsterType;
        MonsterList = DeserializeJson<Monster>("5e-SRD-Monsters.json");
        TreasureList = DeserializeJson<TreasureDescription>("treasures.json");
    }

    public static List<T> DeserializeJson<T>(string fileName)
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
        return CheckPossible() ? CalcEncounter()[9..] : // remove "Monster: "
            "No suitable monsters with this settings";
    }

    private void EncounterInit()
    {
        Difficulty[0] = Constants.Thresholds[PartyLevel, 0] * PartySize;
        Difficulty[1] = Constants.Thresholds[PartyLevel, 1] * PartySize;
        Difficulty[2] = Constants.Thresholds[PartyLevel, 2] * PartySize;
        Difficulty[3] = Constants.Thresholds[PartyLevel, 3] * PartySize;
        FilteredMonsters = MonsterList;
        SumXp = Difficulty[DungeonDifficulty];
    }
    private bool CheckPossible()
    {
        return FilteredMonsters.Any(monster => SumXp > GetMonsterXp(monster));
    }

    private string CalcEncounter()
    {
        var result = new StringBuilder();
        result.Append("Monster: ");
        if (GetRandomInt(0, 101) > 50)
        {
            result.Append(AddMonster(SumXp));
        }
        else
        {
            var x = GetRandomInt(2, DungeonDifficulty + 3);
            for (var i = 0; i < x; i++)
            {
                result.Append(AddMonster(SumXp / x));
                result.Append(", ");
            }
            result.Length -= 2;
        }
        return result.Replace(", None", "").ToString();
    }

    private string AddMonster(int currentXp)
    {
        var monsterCount = FilteredMonsters.Count;
        var monster = 0;
        while (monster < monsterCount)
        {
            var currentMonster = FilteredMonsters[GetRandomInt(0, FilteredMonsters.Count)]; // get random monster
            FilteredMonsters.Remove(currentMonster);
            var monsterXp = GetMonsterXp(currentMonster);
            for (var i = Constants.Multipliers.GetLength(0) - 1; i > -1; i--)
            {
                var count = (int)Constants.Multipliers[i, 0];
                var allXp = monsterXp * count * Constants.Multipliers[i, 1];
                if (allXp <= currentXp && count > 1)
                    return count + "x " + currentMonster.Name + " (CR: " + currentMonster.Challenge_Rating + ") " + monsterXp * count + " XP";
                if (allXp <= currentXp)
                    return currentMonster.Name + " (CR: " + currentMonster.Challenge_Rating + ") " + monsterXp + " XP";
            }
            monster++;
        }
        return "None";
    }
    private static int GetMonsterXp(Monster monster)
    {
        return Constants.ChallengeRatingXp[Constants.ChallengeRating.IndexOf(monster.Challenge_Rating)]; // get monster xp
    }

    public string GetCurrentTrap(bool door)
    {
        var trapSeverity = GetTrapSeverity();
        _currentTrap = door ? Constants.DoorTraps[GetRandomInt(0, Constants.DoorTraps.Count)] : Constants.SimpleTraps[GetRandomInt(0, Constants.SimpleTraps.Count)]; // get random trap
        if (_currentTrap.DmgType.HasValue)
            return _currentTrap.Name + " [" + GetTrapSeverity() + "]: DC " + _currentTrap.Spot + " to spot, DC " + _currentTrap.Disable + " to disable (" + _currentTrap.DisableCheck.GetDescription() + "), DC " + GetTrapSaveDc((int)trapSeverity) + " " + _currentTrap.Save + " save or take " + GetTrapDamage((int)trapSeverity) + "D10 (" + _currentTrap.DmgType + ") damage" + GetTrapAttackBonus((int)trapSeverity);
        return _currentTrap.Name + " [" + GetTrapSeverity() + "]: DC " + _currentTrap.Spot + " to spot, DC " + _currentTrap.Disable + " to disable (" + _currentTrap.DisableCheck.GetDescription() + "), DC " + GetTrapSaveDc((int)trapSeverity) + " " + _currentTrap.Save + " save or " + _currentTrap.Special;
    }

    private string GetTrapAttackBonus(int trapDanger)
    {
        if (!_currentTrap.AttackMod)
            return ".";
        var min = Constants.TrapAttackBonus[trapDanger];
        var max = Constants.TrapAttackBonus[trapDanger + 1];
        return ",\n (attack bonus +" + GetRandomInt(min, max) + ").";
    }

    private int GetTrapDamage(int trapDanger)
    {
        return PartyLevel switch
        {
            < 5 => Constants.TrapDmgSeverity[0, trapDanger],
            < 11 => Constants.TrapDmgSeverity[1, trapDanger],
            < 17 => Constants.TrapDmgSeverity[2, trapDanger],
            _ => Constants.TrapDmgSeverity[3, trapDanger]
        };
    }

    private int GetTrapSaveDc(int trapDanger)
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
            return "Treasures: Empty";

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
        var finalList = new List<TreasureDescription>();
        while (currentCount < itemCount && maxAttempt > 0)
        {
            var currentTreasure = filteredTreasures[GetRandomInt(0, filteredTreasures.Count)];
            if (currentValue + currentTreasure.Cost < SumValue)
            { // if it's still affordable add to list
                currentValue += currentTreasure.Cost;
                finalList.Add(currentTreasure);
                currentCount++;
            }
            maxAttempt--;
        }
        var query = finalList // get occurrence
            .GroupBy(x => x)
            .ToDictionary(x => x.Key, y => y.Count());
        foreach (var item in query)
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

        return TreasureList
            .Where(item => item.Rarity <= ItemsRarity && item.Cost < SumValue && item.Types.Contains(MonsterType))
            .ToList();
    }

    private void GetAllCost()
    {
        SumValue = (int)(Constants.TreasureGp[PartyLevel] * TreasureValue);
    }

    private string GetDoorDescription()
    {
        var sb = new StringBuilder();
        WestCount = 1;
        SouthCount = 1;
        EastCount = 1;
        NorthCount = 1;
        foreach (var door in DoorList)
        {
            var start = sb.Length;
            RoomPosition.CheckRoomPosition(DungeonTiles, door.I, door.J);
            sb.Append(GetDoorText(door.Texture, GetDoorDc()));
            var end = sb.Length;
            DungeonTiles[door.I][door.J].Description = sb.ToString(start, end - start);
        }
        sb.Length -= 1;
        return sb.ToString();
    }

    private string GetDoorText(Textures texture, int x)
    {
        if (RoomPosition.Up)
            return "South Entry #" + SouthCount++ + ": " + Constants.DoorTypes[x] + GetState(texture, x) + "\n";
        if (RoomPosition.Down)
            return "North Entry #" + NorthCount++ + ": " + Constants.DoorTypes[x] + GetState(texture, x) + "\n";
        if (RoomPosition.Right)
            return "West Entry #" + WestCount++ + ": " + Constants.DoorTypes[x] + GetState(texture, x) + "\n";
        return "East Entry #" + EastCount++ + ": " + Constants.DoorTypes[x] + GetState(texture, x) + "\n";
    }

    private string GetState(Textures texture, int x)
    {
        return texture switch
        {
            Textures.NoCorridorDoorLocked or Textures.DoorLocked => " Locked Door (AC " + Constants.DoorAc[x] + ", HP " + Constants.DoorHp[x] + ", DC " + Constants.LockDifficulty[x] + " to unlock)",
            Textures.NoCorridorDoorTrapped or Textures.DoorTrapped => " Trapped Door (AC " + Constants.DoorAc[x] + ", HP " + Constants.DoorHp[x] + ") " + GetCurrentTrap(true),
            _ => " Open Door (AC " + Constants.DoorAc[x] + ", HP " + Constants.DoorHp[x] + ")",
        };
    }

    private int GetDoorDc()
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

    public bool CheckNcDoor(DungeonTile[][] dungeonTiles, int x, int y)
    {
        return dungeonTiles[x][y].Texture is Textures.NoCorridorDoor or Textures.NoCorridorDoorLocked or Textures.NoCorridorDoorTrapped;
    }

    public string GetNcDoorDescription(DungeonTile[][] dungeonTiles, List<DungeonTile> closedList)
    {
        WestCount = 1;
        SouthCount = 1;
        EastCount = 1;
        NorthCount = 1;
        var sb = new StringBuilder();
        foreach (var tile in closedList)
        {
            if (CheckNcDoor(dungeonTiles, tile.I, tile.J - 1))
            {
                sb.Append("West Entry #");
                sb.Append(WestCount++);
                sb.Append(dungeonTiles[tile.I][tile.J - 1].Description);
            }
            else if (CheckNcDoor(dungeonTiles, tile.I, tile.J + 1))
            {
                sb.Append("East Entry #");
                sb.Append(EastCount++);
                sb.Append(dungeonTiles[tile.I][tile.J + 1].Description);
            }
            else if (CheckNcDoor(dungeonTiles, tile.I + 1, tile.J))
            {
                sb.Append("South Entry #");
                sb.Append(SouthCount++);
                sb.Append(dungeonTiles[tile.I + 1][tile.J].Description);
            }
            else if (CheckNcDoor(dungeonTiles, tile.I - 1, tile.J))
            {
                sb.Append("North Entry #");
                sb.Append(NorthCount++);
                sb.Append(dungeonTiles[tile.I - 1][tile.J].Description);
            }
        }
        sb.Length -= 1;
        return sb.ToString();
    }

    public string GetNcDoor(DungeonTile door)
    {
        var doorDc = GetDoorDc();
        return ": " + Constants.DoorTypes[doorDc] + GetState(door.Texture, doorDc) + "\n";
    }
}