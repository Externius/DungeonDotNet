using RDMG.Core.Abstractions.Dungeon.Models;
using RDMG.Core.Abstractions.Services.Models;
using System.Collections.Generic;

namespace RDMG.Core.Abstractions.Dungeon
{
    public interface IDungeonHelper
    {
        int GetRandomInt(int min, int max);
        void AddRoomDescription(DungeonTile[][] dungeonTiles, int x, int y, List<RoomDescription> roomDescription, List<DungeonTile> currentDoors);
        void AddTrapDescription(DungeonTile[][] dungeonTiles, int x, int y, List<TrapDescription> trapDescription);
        void AddRoamingMonsterDescription(DungeonTile[][] dungeonTiles, int x, int y, List<RoamingMonsterDescription> roamingMonsterDescription);
        int Manhattan(int dx, int dy);
        void AddNcRoomDescription(DungeonTile[][] dungeonTiles, int x, int y, List<RoomDescription> roomDescription, string doors);
        void Init(DungeonOptionModel model);
        bool CheckNcDoor(DungeonTile[][] dungeonTiles, int i, int j);
        string GetNcDoorDescription(DungeonTile[][] dungeonTiles, List<DungeonTile> closedList);
        string GetNcDoor(DungeonTile door);
        string GetTreasure();
        string GetMonster();
        string GetCurrentTrap(bool door);
        string GetRoamingMonster();
    }
}
