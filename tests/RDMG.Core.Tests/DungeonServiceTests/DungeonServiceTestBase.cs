using RDMG.Core.Abstractions.Generator;
using RDMG.Core.Abstractions.Services;

namespace RDMG.Core.Tests.DungeonServiceTests;

public abstract class DungeonServiceTestBase
{
    protected readonly IDungeonService DungeonService;
    protected readonly IDungeonNoCorridor DungeonNoCorridor;
    protected DungeonServiceTestBase()
    {
        var env = new TestEnvironment();
        DungeonService = env.GetService<IDungeonService>();
        DungeonNoCorridor = env.GetNcDungeon();
    }
}