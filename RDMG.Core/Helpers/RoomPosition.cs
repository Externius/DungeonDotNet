using RDMG.Core.Abstractions.Generator.Models;
using RDMG.Core.Domain;

namespace RDMG.Core.Helpers;

public static class RoomPosition
{
    public static bool Up { get; set; }
    public static bool Down { get; set; }
    public static bool Left { get; set; }
    public static bool Right { get; set; }
    internal static void CheckRoomPosition(DungeonTile[][] dungeonTiles, int x, int y)
    {
        Up = false;
        Down = false;
        Left = false;
        Right = false;
        if (dungeonTiles[x][y - 1].Texture == Textures.Room) // left
            Left = true;
        if (dungeonTiles[x][y + 1].Texture == Textures.Room) // right
            Right = true;
        if (dungeonTiles[x + 1][y].Texture == Textures.Room) // bottom
            Down = true;
        if (dungeonTiles[x - 1][y].Texture == Textures.Room) // top
            Up = true;
    }
}