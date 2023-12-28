using RDMG.Core.Abstractions.Generator.Models;
using RDMG.Core.Domain;
using System.Collections.Immutable;

namespace RDMG.Core.Generator;

public static class Constants
{
    public const string None = "None";
    public const string Hp = "HP";
    public const string Ac = "AC";
    public const string Dc = "DC";
    public const string Empty = "[]";
    public const string WestEntry = "West Entry #";
    public const string EastEntry = "East Entry #";
    public const string SouthEntry = "South Entry #";
    public const string NorthEntry = "North Entry #";
    public const string LineBreak = "\n";
    public const string ColonWithSpace = ": ";
    public const string MonsterNone = "Monster: None";
    public const string MonsterError = "Monster: No suitable monsters with this settings";

    // door
    public static readonly string[] DoorTypes = [
        "Crystal",
        "Wooden",
        "Stone",
        "Iron",
        "Steel",
        "Mithral",
        "Adamantine"
    ];
    public static readonly int[] DoorAc = [
        13, 15, 17, 19, 19, 21, 23
    ];
    public static readonly int[] DoorHp = [
        10, 10, 15, 15, 18, 18, 27
    ];
    public static readonly int[] LockDifficulty = [
        5, 10, 15, 20, 25, 25, 30
    ];
    // treasure
    public static readonly int[] TreasureGp = [
        0, 300, 600, 900, 1200, 1600, 2000, 2600, 3400, 4500, 5800,
        7500, 9800, 13000, 17000, 22000, 28000, 36000, 47000, 61000, 80000
    ];
    public static readonly int[] ItemCount = [
        0, 4, 4, 5, 5, 7, 7, 8, 8, 8, 9,
        9, 9, 9, 9, 12, 12, 12, 15, 15, 15
    ];
    // trap
    public static readonly int[] TrapSave = [
        10, 12, 16, 21
    ];
    public static readonly int[] TrapAttackBonus = [
        3, 6, 9, 13
    ];
    public static readonly int[,] TrapDmgSeverity = {
        {1, 2, 4},
        {2, 4, 10},
        {4, 10, 18},
        {10, 18, 24}
    };
    public static readonly ImmutableArray<Trap> SimpleTraps =
    [
        new Trap("Collapsing Roof", Save.Dexterity, 10, 15, DisableCheck.Dexterity, false, DamageType.Bludgeoning, ""),
        new Trap("Falling Net", Save.Strength, 10, 15, DisableCheck.Dexterity, false, null, "restrained."),
        new Trap("Fire-Breathing Statue", Save.Dexterity, 15, 13, DisableCheck.DispelMagic, false, DamageType.Fire, ""),
        new Trap("Spiked Pit", Save.Constitution, 15, 15, DisableCheck.Intelligence, false, DamageType.Piercing, ""),
        new Trap("Locking Pit", Save.Strength, 10, 15, DisableCheck.Intelligence, false, null, "locked."),
        new Trap("Poison Darts", Save.Constitution, 15, 15, DisableCheck.Intelligence, true, DamageType.Poison, ""),
        new Trap("Poison Needle", Save.Constitution, 15, 15, DisableCheck.Dexterity, false, DamageType.Poison, ""),
        new Trap("Rolling Sphere", Save.Dexterity, 15, 15, DisableCheck.Intelligence, false, DamageType.Bludgeoning, "")
    ];

    public static readonly ImmutableArray<Trap> DoorTraps =
    [
        new Trap("Fire trap", Save.Dexterity, 10, 15, DisableCheck.Intelligence, false, DamageType.Fire, ""),
        new Trap("Lock Covered in Dragon Bile", Save.Constitution, 10, 15, DisableCheck.Intelligence, false, DamageType.Poison, ""),
        new Trap("Hail of Needles", Save.Dexterity, 15, 13, DisableCheck.Dexterity, false, DamageType.Piercing, ""),
        new Trap("Stone Blocks from Ceiling", Save.Dexterity, 15, 15, DisableCheck.Intelligence, false, DamageType.Bludgeoning, ""),
        new Trap("Doorknob Smeared with Contact Poison", Save.Constitution, 15, 10, DisableCheck.Intelligence, false, DamageType.Poison, ""),
        new Trap("Poison Darts", Save.Constitution, 15, 15, DisableCheck.Intelligence, true, DamageType.Poison, ""),
        new Trap("Poison Needle", Save.Constitution, 15, 15, DisableCheck.Dexterity, false, DamageType.Poison, ""),
        new Trap("Energy Drain", Save.Constitution, 15, 15, DisableCheck.DispelMagic, false, DamageType.Necrotic, "")
    ];

    // encounter
    public static readonly int[] ChallengeRatingXp = [
        10,
        25,
        50,
        100,
        200,
        450,
        700,
        1100,
        1800,
        2300,
        2900,
        3900,
        5000,
        5900,
        7200,
        8400,
        10000,
        11500,
        13000,
        15000,
        18000,
        20000,
        22000,
        25000,
        33000,
        41000,
        50000,
        62000,
        75000,
        90000,
        105000,
        120000,
        135000,
        155000
    ];
    public static readonly double[,] Multipliers = {
        {1, 1},
        {2, 1.5},
        {3, 2},
        {7, 2.5},
        {11, 3},
        {15, 4}
    };

    public static readonly ImmutableArray<string> ChallengeRating =
    [
        "0",
        "1/8",
        "1/4",
        "1/2",
        "1",
        "2",
        "3",
        "4",
        "5",
        "6",
        "7",
        "8",
        "9",
        "10",
        "11",
        "12",
        "13",
        "14",
        "15",
        "16",
        "17",
        "18",
        "19",
        "20",
        "21",
        "22",
        "23",
        "24",
        "25",
        "26",
        "27",
        "28",
        "29",
        "30"
,
    ];

    public static readonly int[,] Thresholds = {
        {0, 0, 0, 0},
        {25, 50, 75, 100},
        {50, 100, 150, 200},
        {75, 150, 225, 400},
        {125, 250, 375, 500},
        {250, 500, 750, 1100},
        {300, 600, 900, 1400},
        {350, 750, 1100, 1700},
        {450, 900, 1400, 2100},
        {550, 1100, 1600, 2400},
        {600, 1200, 1900, 2800},
        {800, 1600, 2400, 3600},
        {1000, 2000, 3000, 4500},
        {1100, 2200, 3400, 5100},
        {1250, 2500, 3800, 5700},
        {1400, 2800, 4300, 6400},
        {1600, 3200, 4800, 7200},
        {2000, 3900, 5900, 8800},
        {2100, 4200, 6300, 9500},
        {2400, 4900, 7300, 10900},
        {2800, 5700, 8500, 12700}
    };
}