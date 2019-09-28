using System;
using System.Collections.Generic;
using System.Text;
using MvcRDMG.Generator.Models;

namespace MvcRDMG.Generator.Helpers
{
    public class Door : IDoor
    {
        public DungeonTile[][] DungeonTiles { get; set; }
        public List<DungeonTile> DoorList { get; set; }
        private int WestCount;
        private int SouthCount;
        private int EastCount;
        private int NorthCount;
        private readonly string[] DoorTypes = {
            "Crystal",
            "Wooden",
            "Stone",
            "Iron",
            "Steel",
            "Mithral",
            "Adamantine"
        };
        private readonly int[] DoorAC = {
                13, 15, 17, 19, 19, 21, 23
        };
        private readonly int[] DoorHP = {
                10, 10, 15, 15, 18, 18, 27
        };
        private readonly int[] LockDifficulty = {
                5, 10, 15, 20, 25, 25, 30
        };
        public Door()
        {

        }


        public string GetDoorDescription()
        {
            var sb = new StringBuilder();
            int start;
            int end;
            WestCount = 1;
            SouthCount = 1;
            EastCount = 1;
            NorthCount = 1;
            foreach (var door in DoorList)
            {
                start = sb.Length;
                RoomPosition.CheckRoomPosition(DungeonTiles, door.I, door.J);
                sb.Append(GetDoorText(door.Texture, GetDoorDC()));
                end = sb.Length;
                DungeonTiles[door.I][door.J].Description = sb.ToString(start, end - start);
            }
            sb.Length -= 1;
            return sb.ToString();
        }
        private string GetDoorText(Textures texture, int x)
        {
            if (RoomPosition.Up)
            {
                return "South Entry #" + SouthCount++ + ": " + DoorTypes[x] + GetState(texture, x) + "\n";
            }
            else if (RoomPosition.Down)
            {
                return "North Entry #" + NorthCount++ + ": " + DoorTypes[x] + GetState(texture, x) + "\n";
            }
            else if (RoomPosition.Right)
            {
                return "West Entry #" + WestCount++ + ": " + DoorTypes[x] + GetState(texture, x) + "\n";
            }
            else
            {
                return "East Entry #" + EastCount++ + ": " + DoorTypes[x] + GetState(texture, x) + "\n";
            }
        }
        private string GetState(Textures texture, int x)
        {
            switch (texture)
            {
                case Textures.NO_CORRIDOR_DOOR_LOCKED:
                case Textures.DOOR_LOCKED:
                    return " Locked Door (AC " + DoorAC[x] + ", HP " + DoorHP[x] + ", DC " + LockDifficulty[x] + " to unlock)";
                case Textures.NO_CORRIDOR_DOOR_TRAPPED:
                case Textures.DOOR_TRAPPED:
                    return " Trapped Door (AC " + DoorAC[x] + ", HP " + DoorHP[x] + ") " + Utils.Instance.TrapGenerator.GetCurrentTrap(true);
                default:
                    return " Open Door (AC " + DoorAC[x] + ", HP " + DoorHP[x] + ")";
            }
        }
        private int GetDoorDC()
        {
            return Utils.Instance.DungeonDifficulty switch
            {
                0 => Utils.Instance.GetRandomInt(0, 2),
                1 => Utils.Instance.GetRandomInt(1, 4),
                2 => Utils.Instance.GetRandomInt(1, 6),
                3 => Utils.Instance.GetRandomInt(2, 7),
                _ => 0,
            };
        }

        public bool CheckNCDoor(DungeonTile[][] dungeonTiles, int x, int y)
        {
            return dungeonTiles[x][y].Texture == Textures.NO_CORRIDOR_DOOR || dungeonTiles[x][y].Texture == Textures.NO_CORRIDOR_DOOR_LOCKED || dungeonTiles[x][y].Texture == Textures.NO_CORRIDOR_DOOR_TRAPPED;
        }

        public string GetNCDoorDescription(DungeonTile[][] dungeonTiles, List<DungeonTile> closedList)
        {
            WestCount = 1;
            SouthCount = 1;
            EastCount = 1;
            NorthCount = 1;
            var sb = new StringBuilder();
            foreach (var tile in closedList)
            {
                if (CheckNCDoor(dungeonTiles, tile.I, tile.J - 1))
                {
                    sb.Append("West Entry #");
                    sb.Append(WestCount++);
                    sb.Append(dungeonTiles[tile.I][tile.J - 1].Description);
                }
                else if (CheckNCDoor(dungeonTiles, tile.I, tile.J + 1))
                {
                    sb.Append("East Entry #");
                    sb.Append(EastCount++);
                    sb.Append(dungeonTiles[tile.I][tile.J + 1].Description);
                }
                else if (CheckNCDoor(dungeonTiles, tile.I + 1, tile.J))
                {
                    sb.Append("South Entry #");
                    sb.Append(SouthCount++);
                    sb.Append(dungeonTiles[tile.I + 1][tile.J].Description);
                }
                else if (CheckNCDoor(dungeonTiles, tile.I - 1, tile.J))
                {
                    sb.Append("North Entry #");
                    sb.Append(NorthCount++);
                    sb.Append(dungeonTiles[tile.I - 1][tile.J].Description);
                }
            }
            sb.Length -= 1;
            return sb.ToString();
        }

        public string GetNCDoor(DungeonTile door)
        {
            int x = GetDoorDC();
            return ": " + DoorTypes[x] + GetState(door.Texture, x) + "\n";
        }
    }
}