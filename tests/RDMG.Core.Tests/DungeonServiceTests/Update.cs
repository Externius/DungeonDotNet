using RDMG.Core.Abstractions.Services;
using RDMG.Core.Generator;
using Shouldly;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RDMG.Core.Tests.DungeonServiceTests;

public class Update
{
    [Fact]
    public async Task CanUpdateDungeon()
    {
        using var env = new TestEnvironment();
        var service = env.GetService<IDungeonService>();
        var source = new CancellationTokenSource();
        var token = source.Token;
        const int dungeonId = 1;
        var oldDungeon = await service.GetDungeonAsync(dungeonId, token);
        var newDungeon = env.GetNcDungeon();
        oldDungeon.DungeonTiles = newDungeon.DungeonTiles.ToString();
        oldDungeon.TrapDescription = Constants.Empty;
        oldDungeon.RoomDescription = newDungeon.RoomDescription.ToString();
        oldDungeon.RoamingMonsterDescription = Constants.Empty;
        await service.UpdateDungeonAsync(oldDungeon, token);
        var result = await service.GetDungeonAsync(dungeonId, token);
        result.RoamingMonsterDescription.ShouldBe(Constants.Empty);
        result.TrapDescription.ShouldBe(Constants.Empty);
        result.DungeonTiles.ShouldBe(newDungeon.DungeonTiles.ToString());
        result.RoomDescription.ShouldBe(newDungeon.RoomDescription.ToString());
    }

    [Fact]
    public async Task CanRenameDungeon()
    {
        using var env = new TestEnvironment();
        var service = env.GetService<IDungeonService>();
        var source = new CancellationTokenSource();
        var token = source.Token;
        const int id = 1;
        var existingOption = await service.GetDungeonOptionAsync(id, token);
        const string newName = "New Name";
        await service.RenameDungeonAsync(existingOption.Id, existingOption.UserId, newName, token);
        var renamed = await service.GetDungeonOptionAsync(id, token);
        renamed.DungeonName.ShouldBe(newName);
    }
}