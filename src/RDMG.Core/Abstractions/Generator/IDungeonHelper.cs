using RDMG.Core.Abstractions.Generator.Models;
using RDMG.Core.Abstractions.Services.Models;
using System.Collections.Generic;

namespace RDMG.Core.Abstractions.Generator;

public interface IDungeonHelper
{
    int GetRandomInt(int min, int max);
    void AddRoomDescription(DungeonTile[][] dungeonTiles, int x, int y, List<RoomDescription> roomDescription, List<DungeonTile> currentDoors);
    void AddTrapDescription(DungeonTile dungeonTile, List<TrapDescription> trapDescription);
    void AddRoamingMonsterDescription(DungeonTile dungeonTile, List<RoamingMonsterDescription> roamingMonsterDescription);
    int Manhattan(int dx, int dy);
    void AddNcRoomDescription(DungeonTile dungeonTile, List<RoomDescription> roomDescription, string doors);
    void Init(DungeonOptionModel model);
    bool CheckNcDoor(DungeonTile dungeonTile);
    string GetNcDoorDescription(DungeonTile[][] dungeonTiles, List<DungeonTile> closedList);
    string GetNcDoor(DungeonTile door);
    string GetTreasure();
    string GetMonster();
    string GetCurrentTrap(bool door);
    string GetRoamingMonster();
    DungeonTile[][] GenerateDungeonTiles(int dungeonSize, int imgSizeX, int imgSizeY);
}