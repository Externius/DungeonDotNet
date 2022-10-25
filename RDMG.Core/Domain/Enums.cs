using System.ComponentModel;

namespace RDMG.Core.Domain
{
    public enum Role
    {
        User = 1,
        Admin
    }
    public enum Item
    {
        Trap = 0,
        RoamingMonster
    }
    public enum Textures
    {
        Edge,
        Marble,
        Corridor,
        Door,
        Room,
        Entry,
        Trap,
        RoomEdge,
        NoCorridorDoor,
        DoorLocked,
        DoorTrapped,
        NoCorridorDoorLocked,
        NoCorridorDoorTrapped,
        RoamingMonster
    }
    public enum Save
    {
        Strength,
        Dexterity,
        Constitution,
        Intelligence,
        Wisdom,
        Charisma
    }
    public enum DisableCheck
    {
        Strength,
        Dexterity,
        Intelligence,
        [Description("Dispel Magic")]
        DispelMagic
    }
    public enum DamageType
    {
        Acid,
        Bludgeoning,
        Cold,
        Fire,
        Necrotic,
        Piercing,
        Poison
    }

    public enum TrapSeverity
    {
        Setback,
        Dangerous,
        Deadly
    }

    public enum OptionKey
    {
        Size,
        Difficulty,
        PartySize,
        PartyLevel,
        TreasureValue,
        ItemsRarity,
        RoomDensity,
        RoomSize,
        MonsterType,
        TrapPercent,
        RoamingPercent,
        Theme
    }
}
