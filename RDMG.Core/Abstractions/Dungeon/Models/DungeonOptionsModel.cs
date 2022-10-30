namespace RDMG.Core.Abstractions.Dungeon.Models;

public class DungeonOptionsModel
{
    public int Width { get; set; } = 800;
    public int Height { get; set; } = 800;
    public int Size { get; set; }
    public int RoomDensity { get; set; }
    public int RoomSizePercent { get; set; }
    public int TrapPercent { get; set; }
    public bool HasDeadEnds { get; set; }
    public int RoamingPercent { get; set; }
}