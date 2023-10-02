namespace RDMG.Core.Domain;

public class Dungeon : AuditableEntity
{
    public string DungeonTiles { get; set; }
    public string RoomDescription { get; set; }
    public string TrapDescription { get; set; }
    public string RoamingMonsterDescription { get; set; }
    public int Level { get; set; }

    public int DungeonOptionId { get; set; }
    public DungeonOption DungeonOption { get; set; }
}