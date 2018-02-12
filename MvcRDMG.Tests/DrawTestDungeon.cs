using System;
using System.Diagnostics;
using MvcRDMG.Generator.Models;

namespace MvcRDMG.Tests
{
    public class DrawTestDungeon
    {
        private static readonly DrawTestDungeon _instance = new DrawTestDungeon();
        public static DrawTestDungeon Instance
        {
            get { return _instance; }
        }
        private DrawTestDungeon()
        {

        }

        public void Draw(DungeonTile[][] dungeonTiles)
        {
            foreach (DungeonTile[] row in dungeonTiles)
            {
                printRow(row);
            }
            Trace.WriteLine(" ");
        }

        private void printRow(DungeonTile[] row)
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
                    default:
                        break;
                }
            }
            Trace.WriteLine(" ");
        }
    }
}