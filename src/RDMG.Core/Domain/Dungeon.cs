namespace RDMG.Core.Domain;

public class Dungeon : AuditableEntity
{
    public string DungeonTiles { get; set; } = string.Empty;
    public string RoomDescription { get; set; } = string.Empty;
    public string TrapDescription { get; set; } = string.Empty;
    public string? RoamingMonsterDescription { get; set; }
    public int Level { get; set; }
    public int DungeonOptionId { get; set; }
    public DungeonOption? DungeonOption { get; set; }
}