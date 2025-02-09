using RDMG.Core.Abstractions.Services;
using RDMG.Core.Generator;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace RDMG.Core.Tests.DungeonServiceTests;

public class Update(TestFixture fixture) : IClassFixture<TestFixture>
{
    private readonly IDungeonService _dungeonService = fixture.DungeonService;

    [Fact]
    public async Task UpdateDungeonAsync_WithValidDungeonModel_UpdatesExistingDungeon()
    {
        const int dungeonId = 4;
        var oldDungeon = await _dungeonService.GetDungeonAsync(dungeonId, TestContext.Current.CancellationToken);
        oldDungeon.DungeonTiles = Constants.Empty;
        oldDungeon.TrapDescription = Constants.Empty;
        oldDungeon.RoomDescription = Constants.Empty;
        oldDungeon.RoamingMonsterDescription = Constants.Empty;
        await _dungeonService.UpdateDungeonAsync(oldDungeon, TestContext.Current.CancellationToken);
        var result = await _dungeonService.GetDungeonAsync(dungeonId, TestContext.Current.CancellationToken);
        result.ShouldNotBeNull();
        result.RoamingMonsterDescription.ShouldBe(Constants.Empty);
        result.TrapDescription.ShouldBe(Constants.Empty);
        result.DungeonTiles.ShouldBe(Constants.Empty);
        result.RoomDescription.ShouldBe(Constants.Empty);
    }

    [Fact]
    public async Task RenameDungeonAsync_WithValidOptionIdAndUserId_RenamesExistingDungeon()
    {
        const int id = 1;
        const string newName = "New Name";
        var existingOption = await _dungeonService.GetDungeonOptionAsync(id, TestContext.Current.CancellationToken);
        await _dungeonService.RenameDungeonAsync(existingOption.Id, existingOption.UserId, newName,
            TestContext.Current.CancellationToken);
        var renamed = await _dungeonService.GetDungeonOptionAsync(id, TestContext.Current.CancellationToken);
        renamed.DungeonName.ShouldBe(newName);
    }
}