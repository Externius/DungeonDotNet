using System.Collections.Generic;
using MvcRDMG.Generator.Models;

namespace MvcRDMG.Generator.Helpers
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