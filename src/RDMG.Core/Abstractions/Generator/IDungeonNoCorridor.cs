using RDMG.Core.Abstractions.Generator.Models;
using RDMG.Core.Abstractions.Services.Models;
using System.Collections.Generic;

namespace RDMG.Core.Abstractions.Generator;
public interface IDungeonNoCorridor
{
    DungeonTile[][] DungeonTiles { get; set; }
    List<RoomDescription> RoomDescription { get; set; }
    List<DungeonTile> OpenDoorList { get; set; }
    DungeonModel Generate(DungeonOptionModel model);
    void AddEntryPoint();
    void Init(DungeonOptionModel optionModel);
    void AddFirstRoom();
    void FillRoomToDoor();
    void AddDescription();
}
