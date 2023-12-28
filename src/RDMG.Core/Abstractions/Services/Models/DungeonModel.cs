
namespace RDMG.Core.Abstractions.Services.Models;

public class DungeonModel : EditModel
{
    public string DungeonTiles { get; set; } = string.Empty;
    public string RoomDescription { get; set; } = string.Empty;
    public string TrapDescription { get; set; } = string.Empty;
    public string? RoamingMonsterDescription { get; set; }
    public int DungeonOptionId { get; set; }
    public int Level { get; set; }
}