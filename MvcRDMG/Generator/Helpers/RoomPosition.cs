using System;
using MvcRDMG.Generator.Models;

namespace MvcRDMG.Generator.Helpers
{
    public class RoomPosition
    {
        private static readonly RoomPosition _instance = new RoomPosition();
        public static RoomPosition Instance { get { return _instance; } }
        public static bool Up { get; set; }
        public static bool Down { get; set; }
        public static bool Left { get; set; }
        public static bool Right { get; set; }
        private RoomPosition()
        {

        }
        internal static void CheckRoomPosition(DungeonTile[][] dungeonTiles, int x, int y)
        {
            Up = false;
            Down = false;
            Left = false;
            Right = false;
            if (dungeonTiles[x][y - 1].Texture == Textures.ROOM)
            { // left
                Left = true;
            }
            if (dungeonTiles[x][y + 1].Texture == Textures.ROOM)
            { // right
                Right = true;
            }
            if (dungeonTiles[x + 1][y].Texture == Textures.ROOM)
            { // bottom
                Down = true;
            }
            if (dungeonTiles[x - 1][y].Texture == Textures.ROOM)
            { // top
                Up = true;
            }
        }
    }
}