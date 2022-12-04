using RDMG.Core.Abstractions.Services;
using Shouldly;
using System.Threading;
using Xunit;

namespace RDMG.Core.Tests.DungeonServiceTests;

public class Update
{
    [Fact]
    public async void CanUpdateDungeon()
    {
        using var env = new TestEnvironment();
        var service = env.GetService<IDungeonService>();
        var source = new CancellationTokenSource();
        var token = source.Token;
        const int dungeonId = 1;
        var oldDungeon = await service.GetDungeonAsync(dungeonId, token);
        var optionModel = await service.GetDungeonOptionAsync(oldDungeon.DungeonOptionId, token);
        optionModel.Corridor = false;
        var newDungeon = await service.GenerateDungeonAsync(optionModel);
        oldDungeon.DungeonTiles = newDungeon.DungeonTiles;
        oldDungeon.TrapDescription = newDungeon.TrapDescription;
        oldDungeon.RoomDescription = newDungeon.RoomDescription;
        oldDungeon.RoamingMonsterDescription = newDungeon.RoamingMonsterDescription;
        await service.UpdateDungeonAsync(oldDungeon, token);

        var result = await service.GetDungeonAsync(dungeonId, token);
        result.RoamingMonsterDescription.ShouldBe("[]");
    }

    [Fact]
    public async void CanRenameDungeon()
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