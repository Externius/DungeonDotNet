using RDMG.Core.Generator;
using Shouldly;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RDMG.Core.Tests.DungeonServiceTests;

public class Update : DungeonServiceTestBase
{
    [Fact]
    public async Task UpdateDungeonAsync_WithValidDungeonModel_UpdatesExistingDungeon()
    {
        var source = new CancellationTokenSource();
        var token = source.Token;
        const int dungeonId = 1;
        var oldDungeon = await DungeonService.GetDungeonAsync(dungeonId, token);
        oldDungeon.DungeonTiles = DungeonNoCorridor.DungeonTiles.ToString();
        oldDungeon.TrapDescription = Constants.Empty;
        oldDungeon.RoomDescription = DungeonNoCorridor.RoomDescription.ToString();
        oldDungeon.RoamingMonsterDescription = Constants.Empty;
        await DungeonService.UpdateDungeonAsync(oldDungeon, token);
        var result = await DungeonService.GetDungeonAsync(dungeonId, token);
        result.RoamingMonsterDescription.ShouldBe(Constants.Empty);
        result.TrapDescription.ShouldBe(Constants.Empty);
        result.DungeonTiles.ShouldBe(DungeonNoCorridor.DungeonTiles.ToString());
        result.RoomDescription.ShouldBe(DungeonNoCorridor.RoomDescription.ToString());
    }

    [Fact]
    public async Task RenameDungeonAsync_WithValidOptionIdAndUserId_RenamesExistingDungeon()
    {
        var source = new CancellationTokenSource();
        var token = source.Token;
        const int id = 1;
        const string newName = "New Name";
        var existingOption = await DungeonService.GetDungeonOptionAsync(id, token);
        await DungeonService.RenameDungeonAsync(existingOption.Id, existingOption.UserId, newName, token);
        var renamed = await DungeonService.GetDungeonOptionAsync(id, token);
        renamed.DungeonName.ShouldBe(newName);
    }
}