using System.ComponentModel.DataAnnotations;

namespace RDMG.Models.Dungeon;

public class DungeonViewModel : EditViewModel
{
    [Required]
    public string DungeonTiles { get; set; }
    [Required]
    public string RoomDescription { get; set; }
    public string TrapDescription { get; set; }
    public string RoamingMonsterDescription { get; set; }
    public int DungeonOptionId { get; set; }
    public int Level { get; set; }
}