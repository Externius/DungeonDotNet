using RDMG.Core.Abstractions.Generator.Models;
using RDMG.Core.Abstractions.Services.Models;
using RDMG.Core.Domain;
using System.Collections.Generic;

namespace RDMG.Core.Abstractions.Generator;
public interface IDungeon
{
    DungeonTile[][] DungeonTiles { get; set; }
    ICollection<RoomDescription> RoomDescription { get; set; }
    DungeonModel Generate(DungeonOptionModel model);
    void AddEntryPoint();
    void Init(DungeonOptionModel optionModel);
    void GenerateCorridors();
    void GenerateRoom();
    void AddDeadEnds();
    void AddCorridorItem(int inCount, Item item);
}
