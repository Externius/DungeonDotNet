
namespace RDMG.Core.Abstractions.Services.Models;

public class DungeonModel : EditModel
{
    public string DungeonTiles { get; set; }
    public string RoomDescription { get; set; }
    public string TrapDescription { get; set; }
    public string RoamingMonsterDescription { get; set; }
    public int DungeonOptionId { get; set; }
    public int Level { get; set; }
}