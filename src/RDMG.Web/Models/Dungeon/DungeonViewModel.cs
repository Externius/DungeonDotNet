using System.ComponentModel.DataAnnotations;

namespace RDMG.Web.Models.Dungeon;

public class DungeonViewModel : EditViewModel
{
    [Required]
    public string DungeonTiles { get; set; } = string.Empty;
    [Required]
    public string RoomDescription { get; set; } = string.Empty;
    public string TrapDescription { get; set; } = string.Empty;
    public string RoamingMonsterDescription { get; set; } = string.Empty;
    public int DungeonOptionId { get; set; }
    public int Level { get; set; }
}