using RDMG.Core.Abstractions.Dungeon.Models;
using System.Collections.Generic;

namespace RDMG.Core.Abstractions.Dungeon
{
    public interface IDoor
    {
        DungeonTile[][] DungeonTiles { get; set; }
        List<DungeonTile> DoorList { get; set; }
        string GetDoorDescription();
        bool CheckNCDoor(DungeonTile[][] dungeonTiles, int i, int j);
        string GetNCDoorDescription(DungeonTile[][] dungeonTiles, List<DungeonTile> closedList);
        string GetNCDoor(DungeonTile door);
    }
}