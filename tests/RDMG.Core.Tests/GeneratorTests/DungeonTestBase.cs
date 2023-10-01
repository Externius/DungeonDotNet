using RDMG.Core.Abstractions.Generator;
using RDMG.Core.Abstractions.Generator.Models;
using RDMG.Core.Domain;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;

namespace RDMG.Core.Tests.GeneratorTests;

public abstract class DungeonTestBase
{
    private readonly ITestOutputHelper _output;
    private readonly StringBuilder _stringBuilder = new();
    protected readonly IDungeon Dungeon;
    protected readonly IDungeonNoCorridor DungeonNoCorridor;
    protected DungeonTestBase(ITestOutputHelper output)
    {
        var env = new TestEnvironment();
        Dungeon = env.GetDungeon();
        DungeonNoCorridor = env.GetNcDungeon();
        _output = output;
    }

    protected void Draw(IEnumerable<DungeonTile[]> dungeonTiles)
    {
        foreach (var row in dungeonTiles)
        {
            PrintRow(row);
        }
        _output.WriteLine(" ");
    }

    private void PrintRow(IEnumerable<DungeonTile> row)
    {
        foreach (var i in row)
        {
            switch (i.Texture)
            {
                case Textures.Edge:
                    _stringBuilder.Append('X');
                    break;
                case Textures.RoomEdge:
                    _stringBuilder.Append('#');
                    break;
                case Textures.Marble:
                    _stringBuilder.Append(' ');
                    break;
                case Textures.Room:
                    _stringBuilder.Append('.');
                    break;
                case Textures.NoCorridorDoor:
                case Textures.NoCorridorDoorLocked:
                case Textures.NoCorridorDoorTrapped:
                case Textures.Door:
                case Textures.DoorLocked:
                case Textures.DoorTrapped:
                    _stringBuilder.Append('D');
                    break;
                case Textures.Corridor:
                    _stringBuilder.Append('-');
                    break;
                case Textures.Entry:
                    _stringBuilder.Append('E');
                    break;
                case Textures.Trap:
                    _stringBuilder.Append('T');
                    break;
                case Textures.RoamingMonster:
                    _stringBuilder.Append('M');
                    break;
                default:
                    _stringBuilder.Append(' ');
                    break;
            }
        }
        _output.WriteLine(_stringBuilder.ToString());
        _stringBuilder.Clear();
    }
}