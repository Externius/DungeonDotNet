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
        TRAP = 0,
        ROAMING_MONSTER
    }
    public enum Textures
    {
        EDGE,
        MARBLE,
        CORRIDOR,
        DOOR,
        ROOM,
        ENTRY,
        TRAP,
        ROOM_EDGE,
        NO_CORRIDOR_DOOR,
        DOOR_LOCKED,
        DOOR_TRAPPED,
        NO_CORRIDOR_DOOR_LOCKED,
        NO_CORRIDOR_DOOR_TRAPPED,
        ROAMING_MONSTER
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
