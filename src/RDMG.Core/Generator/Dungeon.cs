using RDMG.Core.Abstractions.Generator;
using RDMG.Core.Abstractions.Generator.Models;
using RDMG.Core.Abstractions.Services.Models;
using RDMG.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace RDMG.Core.Generator;

public class Dungeon(IDungeonHelper dungeonHelper) : IDungeon
{
    private readonly IDungeonHelper _dungeonHelper = dungeonHelper;
    private const int Movement = 10;
    internal readonly List<DungeonTile> Rooms = [];
    internal List<DungeonTile> Doors = [];
    public ICollection<RoomDescription> RoomDescription { get; set; } = [];
    private ICollection<TrapDescription> TrapDescription { get; set; } = [];
    private ICollection<RoamingMonsterDescription> RoamingMonsterDescription { get; set; } = [];
    public DungeonTile[][] DungeonTiles { get; set; } = [];
    private List<DungeonTile> _result = [];
    private List<DungeonTile> _corridors = [];
    private int _trapCount;
    private int _roamingCount;
    private int _roomCount;
    private bool _hasDeadEnds;
    protected int DungeonWidth { get; set; }
    protected int DungeonHeight { get; set; }
    protected int DungeonSize { get; set; }
    protected int RoomSizePercent { get; set; }
    protected int RoomSize { get; set; }

    public virtual DungeonModel Generate(DungeonOptionModel model)
    {
        Init(model);
        GenerateRoom();
        AddEntryPoint();
        GenerateCorridors();
        if (_hasDeadEnds)
            AddDeadEnds();
        AddCorridorItem(_trapCount, Item.Trap);
        AddCorridorItem(_roamingCount, Item.RoamingMonster);
        return new DungeonModel
        {
            DungeonTiles = JsonSerializer.Serialize(DungeonTiles),
            RoomDescription = JsonSerializer.Serialize(RoomDescription),
            TrapDescription = JsonSerializer.Serialize(TrapDescription),
            RoamingMonsterDescription = JsonSerializer.Serialize(RoamingMonsterDescription),
            DungeonOptionId = model.Id
        };
    }

    public void AddCorridorItem(int inCount, Item item)
    {
        var count = 0;
        while (inCount > count)
        {
            var x = _dungeonHelper.GetRandomInt(0, _corridors.Count);
            var i = _corridors[x].I;
            var j = _corridors[x].J;
            if (DungeonTiles[i][j].Texture != Textures.Corridor)
                continue;
            AddItem(i, j, item);
            count++;
        }
    }

    private void AddItem(int x, int y, Item item)
    {
        switch (item)
        {
            case Item.Trap:
                AddTrap(x, y);
                break;
            case Item.RoamingMonster:
                AddRoamingMonster(x, y);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(item), item, null);
        }
    }

    private void AddTrap(int x, int y)
    {
        DungeonTiles[x][y].Texture = Textures.Trap;
        _dungeonHelper.AddTrapDescription(DungeonTiles[x][y], TrapDescription);
    }

    private void AddRoamingMonster(int x, int y)
    {
        DungeonTiles[x][y].Texture = Textures.RoamingMonster;
        _dungeonHelper.AddRoamingMonsterDescription(DungeonTiles[x][y], RoamingMonsterDescription);
    }

    public void AddDeadEnds()
    {
        var deadEnds = GenerateDeadEnds();
        var firstDoor = Doors[0]; // get first door
        foreach (var end in deadEnds)
        {
            Doors =
            [
                firstDoor,
                end
            ]; // empty doors
            GenerateCorridors();
        }
    }

    private List<DungeonTile> GenerateDeadEnds()
    {
        var count = _roomCount / 2;
        var deadEndsCount = 0;
        var deadEnds = new List<DungeonTile>();
        var croppedDungeonTiles = new DungeonTile[DungeonTiles.Length - 4][];
        for (var i = 0; i < DungeonTiles.Length - 4; i++)
        {
            croppedDungeonTiles[i] = new DungeonTile[DungeonTiles.Length - 4];
        }
        for (var i = 2; i < DungeonTiles.Length - 2; i++)
        {
            Array.Copy(DungeonTiles[i], 2, croppedDungeonTiles[i - 2], 0, DungeonTiles[i].Length - 4);
        }
        var dungeonList = croppedDungeonTiles.SelectMany(T => T).ToList();
        dungeonList.RemoveAll(Rooms.Contains);
        dungeonList.RemoveAll(Doors.Contains);
        dungeonList.RemoveAll(_corridors.Contains);
        var maxAttempt = dungeonList.Count * 2;
        do
        {
            var tile = dungeonList[_dungeonHelper.GetRandomInt(0, dungeonList.Count)];
            if (CheckTileForDeadEnd(tile.I, tile.J))
            {
                DungeonTiles[tile.I][tile.J].Texture = Textures.Corridor;
                deadEnds.Add(DungeonTiles[tile.I][tile.J]);
                deadEndsCount++;
            }
            maxAttempt--;
        }
        while (count != deadEndsCount && maxAttempt > 0);
        return deadEnds;
    }

    private bool CheckTileForDeadEnd(int x, int y)
    {
        for (var i = x - 1; i < x + 2; i++)
        {
            for (var j = y - 1; j < y + 2; j++)
            {
                if (DungeonTiles[i][j].Texture != Textures.Marble) // check if any other tile is there
                    return false;
            }
        }
        return true;
    }

    public void GenerateCorridors()
    {
        for (var d = 0; d < Doors.Count - 1; d++) // -1 because the end point
        {
            _result = new List<DungeonTile>();
            var openList = new List<DungeonTile>();
            var closedList = new List<DungeonTile>();
            var start = Doors[d]; // set door as the starting point
            var end = Doors[d + 1]; // set the next door as the end point
            for (var i = 1; i < DungeonTiles.Length - 1; i++)
            {
                for (var j = 1; j < DungeonTiles.Length - 1; j++) // preconfig H value + restore default values
                {
                    DungeonTiles[i][j].H = _dungeonHelper.Manhattan(Math.Abs(i - end.I), Math.Abs(j - end.J));
                    DungeonTiles[i][j].G = 0;
                    DungeonTiles[i][j].Parent = null;
                    DungeonTiles[i][j].F = 9999;
                }
            }
            AddToClosedList(closedList, start); // add start point to closed list
            AddToOpen(start, openList, closedList, end); // add the nearby nodes to openList
            while (!_result.Any() && openList.Any())
            {
                start = openList[0]; // get lowest F to repeat things (openList sorted)
                AddToClosedList(closedList, start); // add to closed list this node
                RemoveFromOpen(openList, start); // remove from open list this node
                AddToOpen(start, openList, closedList, end); // add open list the nearby nodes
            }
            SetPath(); // modify tiles Texture with the path
        }
    }

    private void SetPath()
    {
        // only change the marble texture
        foreach (var tile in _result.Where(tile => tile.Texture == Textures.Marble))
        {
            DungeonTiles[tile.I][tile.J].Texture = Textures.Corridor;
            _corridors.Add(tile);
        }
    }

    internal static void RemoveFromOpen(List<DungeonTile> openList, DungeonTile node)
    {
        openList.Remove(node);
    }

    private void AddToOpen(DungeonTile node, List<DungeonTile> openList, ICollection<DungeonTile> closedList, DungeonTile end)
    {
        AddToOpenList(node, node.I, node.J - 1, openList, closedList, end); // left
        AddToOpenList(node, node.I - 1, node.J, openList, closedList, end); // top
        AddToOpenList(node, node.I + 1, node.J, openList, closedList, end); // bottom
        AddToOpenList(node, node.I, node.J + 1, openList, closedList, end); // right
        CalcGValue(openList); // calc G value Parent G + Movement
        CalcFValue(openList); // calc F value (G + H)
    }

    private static void CalcFValue(List<DungeonTile> openList)
    {
        openList.ForEach(tile => tile.F = tile.G + tile.H);
        openList.Sort((dt1, dt2) => dt1.F - dt2.F);
    }

    private static void CalcGValue(List<DungeonTile> openList)
    {
        openList.ForEach(tile => tile.G = tile.Parent?.G ?? 0 + Movement);
    }

    private void AddToOpenList(DungeonTile node, int x, int y, ICollection<DungeonTile> openList, ICollection<DungeonTile> closedList, DungeonTile end)
    {
        if (CheckEnd(node, x, y, end))
            return;
        CheckG(node, x, y, openList); // check if it needs reparenting
        if (!CheckTileForOpenList(x, y) || closedList.Contains(DungeonTiles[x][y]) || openList.Contains(DungeonTiles[x][y]))
            return; // not in openlist/closedlist
        SetParent(node, x, y);
        openList.Add(DungeonTiles[x][y]);
    }

    private void SetParent(DungeonTile node, int x, int y)
    {
        DungeonTiles[x][y].Parent = node;
    }

    protected virtual bool CheckTileForOpenList(int x, int y)
    {
        return DungeonTiles[x][y].H != 0 && DungeonTiles[x][y].Texture != Textures.Room && DungeonTiles[x][y].Texture != Textures.RoomEdge; // check its not edge/room/room_edge
    }

    private void CheckG(DungeonTile node, int x, int y, ICollection<DungeonTile> openList)
    {
        if (openList.Contains(DungeonTiles[x][y]) && DungeonTiles[x][y].G > (node.G + Movement))
            SetParent(node, x, y);
    }

    private bool CheckEnd(DungeonTile node, int x, int y, DungeonTile end)
    {
        if (end.I != x || end.J != y)
            return false;
        SetParent(node, x, y);
        GetParents(node);
        return true;
    }

    private void GetParents(DungeonTile node)
    {
        _result.Add(node);
        var parent = node;
        while (parent.Parent != null)
        {
            parent = parent.Parent;
            _result.Add(parent);
        }
    }

    internal void AddToClosedList(List<DungeonTile> closedList, DungeonTile node)
    {
        closedList.Add(DungeonTiles[node.I][node.J]);
    }

    public virtual void AddEntryPoint()
    {
        bool entryIsOk;
        int x;
        int y;
        do
        {
            x = _dungeonHelper.GetRandomInt(1, DungeonTiles.Length - 1);
            y = _dungeonHelper.GetRandomInt(1, DungeonTiles.Length - 1);
            entryIsOk = DungeonTiles[x][y].Texture == Textures.Marble;
        }
        while (!entryIsOk);
        DungeonTiles[x][y].Texture = Textures.Entry;
        Doors.Add(DungeonTiles[x][y]);
    }

    public void GenerateRoom()
    {
        for (var i = 0; i < _roomCount; i++)
        {
            var coordinates = SetTilesForRoom();
            if (coordinates[0] != 0)
                FillRoom(coordinates[0], coordinates[1], coordinates[2], coordinates[3]);
        }
    }

    protected virtual void FillRoom(int x, int y, int right, int down)
    {
        var doorCount = GetDoorCount(down, right);
        for (var i = 0; i < down + 2; i++) // fill with room_edge texture the bigger boundaries
        {
            for (var j = 0; j < right + 2; j++)
            {
                DungeonTiles[x + i - 1][y + j - 1].Texture = Textures.RoomEdge;
            }
        }
        for (var i = 0; i < down; i++) // fill room texture
        {
            for (var j = 0; j < right; j++)
            {
                DungeonTiles[x + i][y + j].Texture = Textures.Room;
                Rooms.Add(DungeonTiles[x + i][y + j]);
                DungeonTiles[x + i][y + j].Description = " ";
                DungeonTiles[x + i][y + j].Index = RoomDescription.Count;
            }
        }
        var currentSize = Doors.Count;
        for (var d = 0; d < doorCount; d++)
        {
            AddDoor(x, y, down, right);
        }
        var newSize = Doors.Count;
        var currentDoors = Doors.GetRange(currentSize, newSize - currentSize);
        _dungeonHelper.AddRoomDescription(DungeonTiles, x, y, RoomDescription, currentDoors);
    }

    internal void AddDoor(int x, int y, int down, int right)
    {
        bool doorIsOk;
        do
        {
            var doorX = _dungeonHelper.GetRandomInt(x, x + down);
            var doorY = _dungeonHelper.GetRandomInt(y, y + right);
            doorIsOk = CheckDoor(doorX, doorY);
        }
        while (!doorIsOk);
    }

    protected virtual bool CheckDoor(int x, int y)
    {
        for (var i = x - 1; i < x + 2; i++)
        {
            for (var j = y - 1; j < y + 2; j++)
            {
                if (DungeonTiles[i][j].Texture is Textures.Door or Textures.DoorLocked or Textures.DoorTrapped) // check nearby doors
                    return false;
            }
        }
        return CheckEnvironment(x, y);
    }

    internal bool CheckEnvironment(int x, int y)
    {
        if (DungeonTiles[x][y - 1].Texture == Textures.RoomEdge) // left
        {
            SetDoor(x, y - 1);
            return true;
        }
        if (DungeonTiles[x][y + 1].Texture == Textures.RoomEdge) // right
        {
            SetDoor(x, y + 1);
            return true;
        }
        if (DungeonTiles[x + 1][y].Texture == Textures.RoomEdge) // bottom
        {
            SetDoor(x + 1, y);
            return true;
        }
        if (DungeonTiles[x - 1][y].Texture == Textures.RoomEdge) // top
        {
            SetDoor(x - 1, y);
            return true;
        }

        return false;
    }

    protected virtual void SetDoor(int x, int y)
    {
        if (_dungeonHelper.GetRandomInt(0, 101) < 40)
            DungeonTiles[x][y].Texture = Textures.DoorTrapped;
        else if (_dungeonHelper.GetRandomInt(0, 101) < 50)
            DungeonTiles[x][y].Texture = Textures.DoorLocked;
        else
            DungeonTiles[x][y].Texture = Textures.Door;
        Doors.Add(DungeonTiles[x][y]);
    }

    protected virtual int GetDoorCount(int down, int right)
    {
        if (down < 4 || right < 4)
            return _dungeonHelper.GetRandomInt(1, 3);
        return _dungeonHelper.GetRandomInt(2, 5);
    }

    private int[] SetTilesForRoom()
    {
        bool roomIsOk;
        int x;
        int y;
        var max = DungeonTiles.Length - (RoomSize + 2); // because of edge + room_edge
        var failSafeCount = DungeonTiles.Length * DungeonTiles.Length / 2;
        int right;
        int down;
        do
        {
            x = _dungeonHelper.GetRandomInt(3, max); // 3 because of edge + room_edge
            y = _dungeonHelper.GetRandomInt(3, max);
            right = _dungeonHelper.GetRandomInt(2, RoomSize + 1);
            down = _dungeonHelper.GetRandomInt(2, RoomSize + 1);
            roomIsOk = CheckTileGoodForRoom(x - 2, y - 2, right + 2, down + 2); // -2/+2 because i want min 3 tiles between rooms
            failSafeCount--;
        }
        while (!roomIsOk && failSafeCount > 0);
        return failSafeCount > 0 ? [x, y, right, down] : [0, 0, 0, 0]; // it can never be 0 if its a good coordinate
    }

    private bool CheckTileGoodForRoom(int x, int y, int right, int down)
    {
        var maxX = x + down + 2; // +2 because of edges
        var maxY = y + right + 2;
        for (var i = x; i < maxX; i++) // check the room area + boundaries
        {
            for (var j = y; j < maxY; j++)
            {
                if (CheckIsRoom(i, j))
                    return false;
            }
        }
        return true;
    }

    private bool CheckIsRoom(int x, int y)
    {
        return DungeonTiles[x][y].Texture is Textures.Room or Textures.RoomEdge;
    }

    public virtual void Init(DungeonOptionModel optionModel)
    {
        _dungeonHelper.Init(optionModel);
        DungeonWidth = optionModel.Width;
        DungeonHeight = optionModel.Height;
        DungeonSize = optionModel.DungeonSize;
        RoomSizePercent = optionModel.RoomSize;
        _hasDeadEnds = optionModel.DeadEnd;
        _corridors = new List<DungeonTile>();
        var imgSizeX = DungeonWidth / DungeonSize;
        var imgSizeY = DungeonHeight / DungeonSize;
        _roomCount = (int)Math.Round((float)DungeonSize / 100 * optionModel.RoomDensity);
        RoomSize = (int)Math.Round((float)(DungeonSize - Math.Round(DungeonSize * 0.35)) / 100 * RoomSizePercent);
        RoomDescription = new List<RoomDescription>();
        TrapDescription = new List<TrapDescription>();
        RoamingMonsterDescription = new List<RoamingMonsterDescription>();
        _trapCount = DungeonSize * optionModel.TrapPercent / 100;
        _roamingCount = DungeonSize * optionModel.RoamingPercent / 100;
        DungeonSize += 2; // because of boundaries
        DungeonTiles = _dungeonHelper.GenerateDungeonTiles(DungeonSize, imgSizeX, imgSizeY);
    }
}