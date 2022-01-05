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
        void AddNCRoomDescription(DungeonTile[][] dungeonTiles, int x, int y, List<RoomDescription> roomDescription, string doors);
        void Init(OptionModel model);
        bool CheckNCDoor(DungeonTile[][] dungeonTiles, int i, int j);
        string GetNCDoorDescription(DungeonTile[][] dungeonTiles, List<DungeonTile> closedList);
        string GetNCDoor(DungeonTile door);
        string GetTreasure();
        string GetMonster();
        string GetCurrentTrap(bool door);
        string GetRoamingMonster();
    }
}
