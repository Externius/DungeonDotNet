using RDMG.Core.Abstractions.Generator;
using RDMG.Core.Abstractions.Generator.Models;
using RDMG.Core.Abstractions.Generator.Models.Json;
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
    // encounter
    private List<Monster> _filteredMonsters;
    private int _sumXp;
    private readonly int[] _difficulty = {
        0, 0, 0, 0
    };
    // trap
    private static Trap _currentTrap;
    // treasure
    private int _sumValue;
    // door
    private DungeonTile[][] DungeonTiles { get; set; }
    private List<DungeonTile> DoorList { get; set; }
    private int _westCount;
    private int _southCount;
    private int _eastCount;
    private int _northCount;
    private static readonly Random Random = new();

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

    public void AddTrapDescription(DungeonTile dungeonTile, List<TrapDescription> trapDescription)
    {
        dungeonTile.Index = trapDescription.Count;
        trapDescription.Add(new TrapDescription(
            GetTrapName(trapDescription.Count + 1),
            GetCurrentTrap(false)));
        dungeonTile.Description = trapDescription.Count.ToString();
    }

    public void AddRoamingMonsterDescription(DungeonTile dungeonTile, List<RoamingMonsterDescription> roamingMonsterDescription)
    {

        dungeonTile.Index = roamingMonsterDescription.Count;
        roamingMonsterDescription.Add(new RoamingMonsterDescription(
            GetRoamingName(roamingMonsterDescription.Count + 1),
            GetRoamingMonster()));
        dungeonTile.Description = roamingMonsterDescription.Count.ToString();
    }

    public int Manhattan(int dx, int dy)
    {
        return dx + dy;
    }

    private static string GetRoomName(int x)
    {
        return "#ROOM" + x + "#";
    }

    public void AddNcRoomDescription(DungeonTile dungeonTile, List<RoomDescription> roomDescription, string doors)
    {

        dungeonTile.Index = roomDescription.Count;
        roomDescription.Add(new RoomDescription(
            GetRoomName(roomDescription.Count + 1),
            GetTreasure(),
            GetMonster(),
            doors));
        dungeonTile.Description = roomDescription.Count.ToString();
    }

    private List<Monster> GetMonsters(IEnumerable<Monster> monsters)
    {
        if (MonsterType.Equals("any", StringComparison.OrdinalIgnoreCase))
        {
            return monsters.Where(monster => Parse(monster.ChallengeRating) <= PartyLevel + 2 &&
                                             Parse(monster.ChallengeRating) >= PartyLevel / 4.0)
                .ToList();
        }

        return monsters.Where(monster => Parse(monster.ChallengeRating) <= PartyLevel + 2 &&
                                         Parse(monster.ChallengeRating) >= PartyLevel / 4.0 &&
                                         MonsterType.Contains(monster.Type))
            .ToList();
    }

    public string GetMonster()
    {
        if (MonsterType.Equals("none", StringComparison.OrdinalIgnoreCase))
            return "Monster: None";
        var checkResult = CheckPossible();
        return checkResult switch
        {
            true when GetRandomInt(0, 101) <= GetMonsterPercentage() => CalcEncounter(),
            false => "Monster: No suitable monsters with this settings",
            _ => "Monster: None"
        };
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
        EncounterInit();
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
        return CheckPossible() ? CalcEncounter()[9..] : // remove "Monster: "
            "No suitable monsters with this settings";
    }

    private void EncounterInit()
    {
        _difficulty[0] = Constants.Thresholds[PartyLevel, 0] * PartySize;
        _difficulty[1] = Constants.Thresholds[PartyLevel, 1] * PartySize;
        _difficulty[2] = Constants.Thresholds[PartyLevel, 2] * PartySize;
        _difficulty[3] = Constants.Thresholds[PartyLevel, 3] * PartySize;
        _filteredMonsters = MonsterList;
        _sumXp = _difficulty[DungeonDifficulty];
    }
    private bool CheckPossible()
    {
        return _filteredMonsters.Any(monster => _sumXp > GetMonsterXp(monster));
    }

    private string CalcEncounter()
    {
        var result = new StringBuilder();
        result.Append("Monster: ");
        if (GetRandomInt(0, 101) > 50)
        {
            result.Append(AddMonster(_sumXp));
        }
        else
        {
            var x = GetRandomInt(2, DungeonDifficulty + 3);
            for (var i = 0; i < x; i++)
            {
                result.Append(AddMonster(_sumXp / x));
                result.Append(", ");
            }
            result.Length -= 2;
        }
        return result.Replace(", None", "").ToString();
    }

    private string AddMonster(int currentXp)
    {
        var monsterCount = _filteredMonsters.Count;
        var monster = 0;
        while (monster < monsterCount)
        {
            var currentMonster = _filteredMonsters[GetRandomInt(0, _filteredMonsters.Count)]; // get random monster
            _filteredMonsters.Remove(currentMonster);
            var monsterXp = GetMonsterXp(currentMonster);
            for (var i = Constants.Multipliers.GetLength(0) - 1; i > -1; i--)
            {
                var count = (int)Constants.Multipliers[i, 0];
                var allXp = monsterXp * count * Constants.Multipliers[i, 1];
                if (allXp <= currentXp && count > 1)
                    return count + "x " + currentMonster.Name + " (CR: " + currentMonster.ChallengeRating + ") " + monsterXp * count + " XP";
                if (allXp <= currentXp)
                    return currentMonster.Name + " (CR: " + currentMonster.ChallengeRating + ") " + monsterXp + " XP";
            }
            monster++;
        }
        return "None";
    }
    private static int GetMonsterXp(Monster monster)
    {
        return Constants.ChallengeRatingXp[Constants.ChallengeRating.IndexOf(monster.ChallengeRating)]; // get monster xp
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
            if (currentValue + currentTreasure.Cost < _sumValue)
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
        sb.Append(GetRandomInt(1, _sumValue - currentValue)); // get the remaining value randomly
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
                .Where(item => item.Rarity <= ItemsRarity && item.Cost < _sumValue)
                .ToList();
        }

        return TreasureList
            .Where(item => item.Rarity <= ItemsRarity && item.Cost < _sumValue && item.Types.Contains(MonsterType))
            .ToList();
    }

    private void GetAllCost()
    {
        _sumValue = (int)(Constants.TreasureGp[PartyLevel] * TreasureValue);
    }

    private string GetDoorDescription()
    {
        var sb = new StringBuilder();
        _westCount = 1;
        _southCount = 1;
        _eastCount = 1;
        _northCount = 1;
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
            return "South Entry #" + _southCount++ + ": " + Constants.DoorTypes[x] + GetState(texture, x) + "\n";
        if (RoomPosition.Down)
            return "North Entry #" + _northCount++ + ": " + Constants.DoorTypes[x] + GetState(texture, x) + "\n";
        if (RoomPosition.Right)
            return "West Entry #" + _westCount++ + ": " + Constants.DoorTypes[x] + GetState(texture, x) + "\n";
        return "East Entry #" + _eastCount++ + ": " + Constants.DoorTypes[x] + GetState(texture, x) + "\n";
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

    public bool CheckNcDoor(DungeonTile dungeonTile)
    {
        return dungeonTile.Texture is Textures.NoCorridorDoor or Textures.NoCorridorDoorLocked or Textures.NoCorridorDoorTrapped;
    }

    public string GetNcDoorDescription(DungeonTile[][] dungeonTiles, List<DungeonTile> closedList)
    {
        _westCount = 1;
        _southCount = 1;
        _eastCount = 1;
        _northCount = 1;
        var sb = new StringBuilder();
        foreach (var tile in closedList)
        {
            if (CheckNcDoor(dungeonTiles[tile.I][tile.J - 1]))
            {
                sb.Append("West Entry #");
                sb.Append(_westCount++);
                sb.Append(dungeonTiles[tile.I][tile.J - 1].Description);
            }
            else if (CheckNcDoor(dungeonTiles[tile.I][tile.J + 1]))
            {
                sb.Append("East Entry #");
                sb.Append(_eastCount++);
                sb.Append(dungeonTiles[tile.I][tile.J + 1].Description);
            }
            else if (CheckNcDoor(dungeonTiles[tile.I + 1][tile.J]))
            {
                sb.Append("South Entry #");
                sb.Append(_southCount++);
                sb.Append(dungeonTiles[tile.I + 1][tile.J].Description);
            }
            else if (CheckNcDoor(dungeonTiles[tile.I - 1][tile.J]))
            {
                sb.Append("North Entry #");
                sb.Append(_northCount++);
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

    public DungeonTile[][] GenerateDungeonTiles(int dungeonSize, int imgSizeX, int imgSizeY)
    {
        var dungeonTiles = new DungeonTile[dungeonSize][];
        for (var i = 0; i < dungeonSize; i++)
        {
            dungeonTiles[i] = new DungeonTile[dungeonSize];
        }

        for (var i = 0; i < dungeonSize; i++)
        {
            for (var j = 0; j < dungeonSize; j++)
            {
                dungeonTiles[i][j] = new DungeonTile(i, j);
            }
        }

        for (var i = 1; i < dungeonSize - 1; i++) // set drawing area
        {
            for (var j = 1; j < dungeonSize - 1; j++)
            {
                dungeonTiles[i][j] = new DungeonTile((j - 1) * imgSizeX, (i - 1) * imgSizeY, i, j, imgSizeX, imgSizeY,
                    Textures.Marble);
            }
        }

        return dungeonTiles;
    }
}