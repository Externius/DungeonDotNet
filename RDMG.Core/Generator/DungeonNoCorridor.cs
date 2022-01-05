using RDMG.Core.Abstractions.Dungeon;
using RDMG.Core.Abstractions.Dungeon.Models;
using RDMG.Core.Domain;
using RDMG.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RDMG.Core.Generator
{
    public class DungeonNoCorridor : Dungeon
    {
        private readonly IDungeonHelper _dungeonHelper;
        public List<DungeonTile> OpenDoorList { get; set; }
        private List<DungeonTile> EdgeTileList;
        private List<DungeonTile> RoomStart;
        public DungeonNoCorridor(int dungeonWidth, int dungeonHeight, int dungeonSize, int roomSizePercent, IDungeonHelper dungeonHelper) : base(dungeonHelper)
        {
            DungeonWidth = dungeonWidth;
            DungeonHeight = dungeonHeight;
            DungeonSize = dungeonSize;
            RoomSizePercent = roomSizePercent;
            _dungeonHelper = dungeonHelper;
        }

        public override void Generate()
        {
            Init();
            AddFirstRoom();
            FillRoomToDoor();
            AddEntryPoint();
            AddDescription();
        }

        public override void Init()
        {
            var ImgSizeX = DungeonWidth / DungeonSize;
            var ImgSizeY = DungeonHeight / DungeonSize;
            RoomSize = (int)Math.Round((float)(DungeonSize - Math.Round(DungeonSize * 0.35)) / 100 * RoomSizePercent);
            RoomDescription = new List<RoomDescription>();
            DungeonSize += 2; // because of boundaries
            DungeonTiles = new DungeonTile[DungeonSize][];
            for (var i = 0; i < DungeonSize; i++)
            {
                DungeonTiles[i] = new DungeonTile[DungeonSize];
            }
            for (var i = 0; i < DungeonSize; i++)
            {
                for (var j = 0; j < DungeonSize; j++)
                {
                    DungeonTiles[i][j] = new DungeonTile(i, j);
                }
            }
            for (var i = 1; i < DungeonSize - 1; i++) // set drawing area
            {
                for (var j = 1; j < DungeonSize - 1; j++)
                {
                    DungeonTiles[i][j] = new DungeonTile((j - 1) * ImgSizeX, (i - 1) * ImgSizeY, i, j, ImgSizeX, ImgSizeY, Textures.MARBLE);
                }
            }
        }

        public override void AddEntryPoint()
        {
            bool entryIsOk;
            int x;
            int y;
            do
            {
                x = _dungeonHelper.GetRandomInt(1, DungeonTiles.Length - 1);
                y = _dungeonHelper.GetRandomInt(1, DungeonTiles.Length - 1);
                RoomPosition.CheckRoomPosition(DungeonTiles, x, y);
                entryIsOk = DungeonTiles[x][y].Texture == Textures.ROOM_EDGE && CheckPos() && CheckNearbyDoor(DungeonTiles[x][y]) && CheckEdges(DungeonTiles, x, y);
            }
            while (!entryIsOk);
            DungeonTiles[x][y].Texture = Textures.ENTRY;
        }

        private static bool CheckEdges(DungeonTile[][] dungeonTiles, int x, int y)
        {
            return dungeonTiles[x][y - 1].Texture == Textures.ROOM ||
                   dungeonTiles[x][y + 1].Texture == Textures.ROOM ||
                   dungeonTiles[x + 1][y].Texture == Textures.ROOM ||
                   dungeonTiles[x - 1][y].Texture == Textures.ROOM;
        }

        public void AddDescription()
        {
            CleanDoorList();
            SetDoorsDescriptions();
            foreach (var room in RoomStart)
            {
                var openList = new List<DungeonTile>();
                var closedList = new List<DungeonTile>();
                var start = room;
                AddToClosedList(closedList, start); // add start point to closed list
                AddToOpen(start, openList, closedList); // add the nearby nodes to openList
                while (openList.Any())
                {
                    start = openList[0]; // get next room
                    AddToClosedList(closedList, start); // add to closed list this node
                    RemoveFromOpen(openList, start); // remove from open list this node
                    AddToOpen(start, openList, closedList); // add open list the nearby nodes
                }
                _dungeonHelper.AddNCRoomDescription(DungeonTiles, room.I, room.J, RoomDescription, _dungeonHelper.GetNCDoorDescription(DungeonTiles, closedList));
            }
        }

        private void AddToOpen(DungeonTile node, List<DungeonTile> openList, List<DungeonTile> closedList)
        {
            AddToOpenList(node.I, node.J - 1, openList, closedList); // left
            AddToOpenList(node.I - 1, node.J, openList, closedList); // top
            AddToOpenList(node.I + 1, node.J, openList, closedList); // bottom
            AddToOpenList(node.I, node.J + 1, openList, closedList); // right
        }

        private void AddToOpenList(int x, int y, List<DungeonTile> openList, List<DungeonTile> closedList)
        {
            if (CheckTileForOpenList(x, y) && !closedList.Contains(DungeonTiles[x][y]) && !openList.Contains(DungeonTiles[x][y])) // not in openlist/closedlist
                openList.Add(DungeonTiles[x][y]);
        }

        private void SetDoorsDescriptions()
        {
            foreach (var door in Doors)
            {
                door.Description = (_dungeonHelper.GetNCDoor(door));
            }
        }

        private void CleanDoorList()
        {
            var toDelete = new List<DungeonTile>();
            foreach (var door in Doors)
            {
                if (door.Texture == Textures.ROOM_EDGE)
                    toDelete.Add(door);
            }
            Doors.RemoveAll(i => toDelete.Contains(i));
        }

        public void FillRoomToDoor()
        {
            while (OpenDoorList.Any())
            {
                var i = OpenDoorList.Count - 1;
                RoomPosition.CheckRoomPosition(DungeonTiles, OpenDoorList[i].I, OpenDoorList[i].J);
                if (CheckPos())
                {
                    if (RoomPosition.Up)
                        RandomFillUpDown(OpenDoorList[i].I + 1, OpenDoorList[i].J, OpenDoorList[i]);
                    else if (RoomPosition.Down)
                        RandomFillUpDown(OpenDoorList[i].I - 1, OpenDoorList[i].J, OpenDoorList[i]);
                    else if (RoomPosition.Right)
                        RandomFillLeftRight(OpenDoorList[i].I, OpenDoorList[i].J - 1, OpenDoorList[i]);
                    else if (RoomPosition.Left)
                        RandomFillLeftRight(OpenDoorList[i].I, OpenDoorList[i].J + 1, OpenDoorList[i]);
                }
                OpenDoorList.Remove(OpenDoorList[i]);
            }
        }

        private void RandomFillLeftRight(int x, int y, DungeonTile door)
        {
            var result = CheckArea(x, y, door);
            if (result[0] != 0 && result[1] != 0)
                FillLeftRight(x, y, result[0], result[1]);
        }

        private void FillLeftRight(int x, int y, int down, int right)
        {
            EdgeTileList = new List<DungeonTile>();
            if (right < 0)
            {
                for (var i = right + 1; i < 1; i++)
                {
                    FillVertical(x, y + i, down);
                }
            }
            else
            {
                for (var i = 0; i < right; i++)
                {
                    FillVertical(x, y + i, down);
                }
            }
            SetVerticalEdge(x, y, right, down);
            SetVerticalEdge(x, y, right < 0 ? 1 : -1, down);
            FillDoor(down, right);
            RoomStart.Add(DungeonTiles[x][y]);
        }

        private void SetVerticalEdge(int x, int y, int right, int down)
        {
            var addToEdgeList = !(right == 1 || right == -1);
            if (down < 0) // up
            {
                for (var i = down; i < 2; i++)
                {
                    SetRoomEdge(x + i, y + right, addToEdgeList); // right edge
                }
            }
            else // bottom
            {
                for (var i = -1; i < down + 1; i++)
                {
                    SetRoomEdge(x + i, y + right, addToEdgeList); // left edge
                }
            }
        }

        private void FillVertical(int x, int y, int down)
        {
            if (down < 0) // up
            {
                for (var i = down + 1; i < 1; i++)
                {
                    SetRoomTiles(x + i, y); // set room
                }
                SetTextureToRoomEdge(x + 1, y); // bottom edge
                AddEdgeTileList(x + 1, y);
            }
            else // down
            {
                for (var i = 0; i < down; i++)
                {
                    SetRoomTiles(x + i, y); // set room
                }
                SetTextureToRoomEdge(x - 1, y); // top edge
                AddEdgeTileList(x - 1, y);
            }
            SetTextureToRoomEdge(x + down, y);
            AddEdgeTileList(x + down, y);
        }

        private void RandomFillUpDown(int x, int y, DungeonTile door)
        {
            var result = CheckArea(x, y, door);
            if (result[0] != 0 && result[1] != 0)
                FillUpDown(x, y, result[0], result[1]);
        }

        private void FillUpDown(int x, int y, int down, int right)
        {
            EdgeTileList = new List<DungeonTile>();
            if (down < 0)
            {
                for (var i = down + 1; i < 1; i++)
                {
                    FillHorizontal(x + i, y, right);
                }
            }
            else
            {
                for (var i = 0; i < down; i++)
                {
                    FillHorizontal(x + i, y, right);
                }
            }
            SetHorizontalEdge(x, y, right, down);
            SetHorizontalEdge(x, y, right, down < 0 ? 1 : -1);
            FillDoor(down, right);
            RoomStart.Add(DungeonTiles[x][y]);
        }

        private void FillDoor(int down, int right)
        {
            var doorCount = GetDoorCount(down, right);
            CleanEdgeTileList();
            var maxTryNumber = EdgeTileList.Count;
            do
            {
                var random = _dungeonHelper.GetRandomInt(0, EdgeTileList.Count);
                if (CheckNearbyDoor(EdgeTileList[random]))
                {
                    SetDoor(EdgeTileList[random].I, EdgeTileList[random].J);
                    doorCount--;
                }
                maxTryNumber--;
            }
            while (doorCount > 0 && maxTryNumber > 0);
        }

        private bool CheckNearbyDoor(DungeonTile node)
        {
            for (var i = node.I - 1; i < node.I + 2; i++)
            {
                for (var j = node.J - 1; j < node.J + 2; j++)
                {
                    if (_dungeonHelper.CheckNCDoor(DungeonTiles, i, j)) // check nearby doors
                        return false;
                }
            }
            return true;
        }

        private void CleanEdgeTileList()
        {
            var toDelete = new List<DungeonTile>();
            foreach (var tile in EdgeTileList)
            {
                if (CheckRooms(tile.I, tile.J)) // if its on the edge
                    toDelete.Add(tile);
            }
            EdgeTileList.RemoveAll(i => toDelete.Contains(i));
        }

        private bool CheckRooms(int x, int y)
        {
            return DungeonTiles[x][y - 1].Texture != Textures.ROOM &&
                DungeonTiles[x][y + 1].Texture != Textures.ROOM &&
                DungeonTiles[x + 1][y].Texture != Textures.ROOM &&
                DungeonTiles[x - 1][y].Texture != Textures.ROOM;
        }

        private void SetHorizontalEdge(int x, int y, int right, int down)
        {
            var addToEdgeList = !(down == 1 || down == -1);
            if (right < 0) // left
            {
                for (var i = right; i < 2; i++)
                {
                    SetRoomEdge(x + down, y + i, addToEdgeList);
                }
            }
            else // right
            {
                for (var i = -1; i < right + 1; i++)
                {
                    SetRoomEdge(x + down, y + i, addToEdgeList);
                }
            }
        }

        private void SetRoomEdge(int x, int y, bool addToEdgeList)
        {
            if (!_dungeonHelper.CheckNCDoor(DungeonTiles, x, y) && addToEdgeList) // if its not a corridor_door
            {
                SetTextureToRoomEdge(x, y);
                AddEdgeTileList(x, y);
            }
            else if (!_dungeonHelper.CheckNCDoor(DungeonTiles, x, y))
            {
                SetTextureToRoomEdge(x, y);
            }
        }

        private void FillHorizontal(int x, int y, int right)
        {
            if (right < 0) // left
            {
                for (var i = right + 1; i < 1; i++)
                {
                    SetRoomTiles(x, y + i); // set room
                }
                SetTextureToRoomEdge(x, y + 1); // right edge
                AddEdgeTileList(x, y + 1);
            }
            else // right
            {
                for (var i = 0; i < right; i++)
                {
                    SetRoomTiles(x, y + i); // set room
                }
                SetTextureToRoomEdge(x, y - 1); // left edge
                AddEdgeTileList(x, y - 1);
            }
            SetTextureToRoomEdge(x, y + right);
            AddEdgeTileList(x, y + right);
        }

        private void AddEdgeTileList(int x, int y)
        {
            EdgeTileList.Add(DungeonTiles[x][y]);
        }

        private void SetTextureToRoomEdge(int x, int y)
        {
            DungeonTiles[x][y].Texture = Textures.ROOM_EDGE;
        }

        private int[] CheckArea(int x, int y, DungeonTile door)
        {
            var vertical = CheckVertical(x, y);
            var horizontal = CheckHorizontal(x, y);
            if (CheckPossible(vertical, horizontal, door))
            {
                var result = GetDownRight(vertical, horizontal);
                var down = result[0];
                var right = result[1];
                vertical = CheckVerticalOneWay(x, y + right, down); // check horizontal end vertically
                horizontal = CheckHorizontalOneWay(x + down, y, right); // check vertical end horizontally
                if (CheckPossibleEnd(vertical, horizontal, door, down, right))
                    return new int[] { down, right };
            }
            return new int[] { 0, 0 };
        }

        private bool CheckPossibleEnd(int vertical, int horizontal, DungeonTile door, int down, int right)
        {
            if (vertical < 0 != down < 0 || horizontal < 0 != right < 0 ||
                Math.Abs(vertical) < Math.Abs(down) ||
                Math.Abs(horizontal) < Math.Abs(right))
            { // it would overlap with another room
                DungeonTiles[door.I][door.J].Texture = Textures.ROOM_EDGE; // change the door to a room_edge
                return false;
            }
            return true;
        }

        private int CheckHorizontalOneWay(int x, int y, int right)
        {
            int count;
            if (right < 0) // left
                count = CheckLeft(x, y);
            else
                count = CheckRight(x, y);
            if (Math.Abs(count) > 2)
                return count;
            return 0;
        }

        private int CheckVerticalOneWay(int x, int y, int down)
        {
            int count;
            if (down < 0) // up
                count = CheckUp(x, y);
            else
                count = CheckDown(x, y);
            if (Math.Abs(count) > 2)
                return count;
            return 0;
        }

        private int[] GetDownRight(int vertical, int horizontal)
        {
            var down = _dungeonHelper.GetRandomInt(2, (Math.Abs(vertical)) > RoomSize ? RoomSize : Math.Abs(vertical));
            var right = _dungeonHelper.GetRandomInt(2, (Math.Abs(horizontal)) > RoomSize ? RoomSize : Math.Abs(horizontal));
            if (vertical < 0)
                down = -down;
            if (horizontal < 0)
                right = -right;
            return new int[] { down, right };
        }

        private bool CheckPossible(int vertical, int horizontal, DungeonTile door)
        {
            if (vertical == 0 || horizontal == 0) // its impossible to add room
            {
                DungeonTiles[door.I][door.J].Texture = Textures.ROOM_EDGE; // change the door to a room_edge
                return false;
            }
            return true;
        }

        private int CheckHorizontal(int x, int y)
        {
            return GetCheckResult(CheckLeft(x, y), CheckRight(x, y));
        }

        private int CheckLeft(int x, int y)
        {
            bool tile;
            bool edge;
            var temp = y;
            var count = 0;
            do
            {
                tile = CheckTile(x, temp);
                edge = CheckDungeonTilesEdge(x, temp);
                temp--;
                count--;
            }
            while (!tile && !edge);
            if (edge)
                count++;
            return count;
        }

        private int CheckRight(int x, int y)
        {
            bool tile;
            bool edge;
            var temp = y;
            var count = 0;
            do
            {
                tile = CheckTile(x, temp);
                edge = CheckDungeonTilesEdge(x, temp);
                temp++;
                count++;
            }
            while (!tile && !edge);
            if (edge)
                count--;
            return count;
        }

        private int CheckVertical(int x, int y)
        {
            return GetCheckResult(CheckUp(x, y), CheckDown(x, y));
        }

        private static int GetCheckResult(int x, int y)
        {
            if (Math.Abs(x) >= Math.Abs(y) && Math.Abs(x) > 2)
                return x;
            if (Math.Abs(y) > Math.Abs(x) && Math.Abs(y) > 2)
                return y;
            return 0;
        }

        private int CheckUp(int x, int y)
        {
            bool tile;
            bool edge;
            var temp = x;
            var count = 0;
            do
            {
                tile = CheckTile(temp, y);
                edge = CheckDungeonTilesEdge(temp, y);
                temp--;
                count--;
            }
            while (!tile && !edge);
            if (edge)
                count++;
            return count;
        }

        private int CheckDown(int x, int y)
        {
            bool tile;
            bool edge;
            var temp = x;
            var count = 0;
            do
            {
                tile = CheckTile(temp, y);
                edge = CheckDungeonTilesEdge(temp, y);
                temp++;
                count++;
            }
            while (!tile && !edge);
            if (edge)
                count--;
            return count;
        }

        private bool CheckDungeonTilesEdge(int x, int y)
        {
            return DungeonTiles[x][y].Texture == Textures.EDGE;
        }

        private bool CheckTile(int x, int y)
        {
            return DungeonTiles[x][y].Texture == Textures.ROOM_EDGE || _dungeonHelper.CheckNCDoor(DungeonTiles, x, y);
        }

        private static bool CheckPos()
        {
            return !(RoomPosition.Up && RoomPosition.Down || RoomPosition.Left && RoomPosition.Right);
        }

        public void AddFirstRoom()
        {
            RoomStart = new List<DungeonTile>();
            OpenDoorList = new List<DungeonTile>();
            var x = _dungeonHelper.GetRandomInt(5, DungeonTiles.Length - (RoomSize + 4));
            var y = _dungeonHelper.GetRandomInt(5, DungeonTiles.Length - (RoomSize + 4));
            var right = _dungeonHelper.GetRandomInt(2, RoomSize + 1);
            var down = _dungeonHelper.GetRandomInt(2, RoomSize + 1);
            FillRoom(x, y, down, right);
            RoomStart.Add(DungeonTiles[x][y]);
        }

        private void SetRoomTiles(int x, int y)
        {
            DungeonTiles[x][y].Texture = Textures.ROOM;
            DungeonTiles[x][y].Description = " ";
            DungeonTiles[x][y].Index = RoomStart.Count;
        }

        internal override void FillRoom(int x, int y, int right, int down)
        {
            var doorCount = GetDoorCount(down, right);
            for (var i = 0; i < down + 2; i++) // fill with room_edge texture the bigger boundaries
            {
                for (var j = 0; j < right + 2; j++)
                {
                    DungeonTiles[x + i - 1][y + j - 1].Texture = Textures.ROOM_EDGE;
                }
            }
            for (var i = 0; i < down; i++) // fill room texture
            {
                for (var j = 0; j < right; j++)
                {
                    SetRoomTiles(x + i, y + j);
                    Rooms.Add(DungeonTiles[x + i][y + j]);
                }
            }
            for (var d = 0; d < doorCount; d++)
            {
                AddDoor(x, y, down, right);
            }
        }

        internal override int GetDoorCount(int down, int right)
        {
            if (Math.Abs(down) < 4 || Math.Abs(right) < 4)
                return 2;
            else
                return _dungeonHelper.GetRandomInt(3, 6);
        }

        internal override bool CheckTileForOpenList(int x, int y)
        {
            return DungeonTiles[x][y].Texture == Textures.ROOM;
        }

        internal override bool CheckDoor(int x, int y)
        {
            for (var i = x - 1; i < x + 2; i++)
            {
                for (var j = y - 1; j < y + 2; j++)
                {
                    if (_dungeonHelper.CheckNCDoor(DungeonTiles, i, j)) // check nearby doors
                        return false;
                }
            }
            return CheckEnvironment(x, y);
        }

        internal override void SetDoor(int x, int y)
        {
            if (_dungeonHelper.GetRandomInt(0, 101) < 40)
                DungeonTiles[x][y].Texture = Textures.NO_CORRIDOR_DOOR_TRAPPED;
            else if (_dungeonHelper.GetRandomInt(0, 101) < 50)
                DungeonTiles[x][y].Texture = Textures.NO_CORRIDOR_DOOR_LOCKED;
            else
                DungeonTiles[x][y].Texture = Textures.NO_CORRIDOR_DOOR;
            OpenDoorList.Add(DungeonTiles[x][y]);
            Doors.Add(DungeonTiles[x][y]);
        }
    }
}