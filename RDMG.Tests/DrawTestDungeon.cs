using RDMG.Core.Abstractions.Dungeon.Models;
using RDMG.Core.Domain;
using System.Diagnostics;

namespace RDMG.Tests
{
    public class DrawTestDungeon
    {
        public static DrawTestDungeon Instance { get; } = new DrawTestDungeon();
        private DrawTestDungeon()
        {

        }
        public static void Draw(DungeonTile[][] dungeonTiles)
        {
            foreach (DungeonTile[] row in dungeonTiles)
            {
                PrintRow(row);
            }
            Trace.WriteLine(" ");
        }
        private static void PrintRow(DungeonTile[] row)
        {
            foreach (var i in row)
            {
                switch (i.Texture)
                {
                    case Textures.EDGE:
                        Trace.Write("X");
                        break;
                    case Textures.ROOM_EDGE:
                        Trace.Write("#");
                        break;
                    case Textures.MARBLE:
                        Trace.Write(" ");
                        break;
                    case Textures.ROOM:
                        Trace.Write(".");
                        break;
                    case Textures.NO_CORRIDOR_DOOR:
                    case Textures.NO_CORRIDOR_DOOR_LOCKED:
                    case Textures.NO_CORRIDOR_DOOR_TRAPPED:
                    case Textures.DOOR:
                    case Textures.DOOR_LOCKED:
                    case Textures.DOOR_TRAPPED:
                        Trace.Write("D");
                        break;
                    case Textures.CORRIDOR:
                        Trace.Write("-");
                        break;
                    case Textures.ENTRY:
                        Trace.Write("E");
                        break;
                    case Textures.TRAP:
                        Trace.Write("T");
                        break;
                    case Textures.ROAMING_MONSTER:
                        Trace.Write("M");
                        break;
                    default:
                        break;
                }
            }
            Trace.WriteLine(" ");
        }
    }
}