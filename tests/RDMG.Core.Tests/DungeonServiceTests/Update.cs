using RDMG.Core.Abstractions.Services;
using RDMG.Core.Generator;
using Shouldly;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RDMG.Core.Tests.DungeonServiceTests;

public class Update(TestFixture fixture) : IClassFixture<TestFixture>
{
    private readonly IDungeonService _dungeonService = fixture.DungeonService;

    [Fact]
    public async Task UpdateDungeonAsync_WithValidDungeonModel_UpdatesExistingDungeon()
    {
        using var source = new CancellationTokenSource();
        var token = source.Token;
        const int dungeonId = 4;
        var oldDungeon = await _dungeonService.GetDungeonAsync(dungeonId, token);
        oldDungeon.DungeonTiles = Constants.Empty;
        oldDungeon.TrapDescription = Constants.Empty;
        oldDungeon.RoomDescription = Constants.Empty;
        oldDungeon.RoamingMonsterDescription = Constants.Empty;
        await _dungeonService.UpdateDungeonAsync(oldDungeon, token);
        var result = await _dungeonService.GetDungeonAsync(dungeonId, token);
        result.ShouldNotBeNull();
        result.RoamingMonsterDescription.ShouldBe(Constants.Empty);
        result.TrapDescription.ShouldBe(Constants.Empty);
        result.DungeonTiles.ShouldBe(Constants.Empty);
        result.RoomDescription.ShouldBe(Constants.Empty);
    }

    [Fact]
    public async Task RenameDungeonAsync_WithValidOptionIdAndUserId_RenamesExistingDungeon()
    {
        using var source = new CancellationTokenSource();
        var token = source.Token;
        const int id = 1;
        const string newName = "New Name";
        var existingOption = await _dungeonService.GetDungeonOptionAsync(id, token);
        await _dungeonService.RenameDungeonAsync(existingOption.Id, existingOption.UserId, newName, token);
        var renamed = await _dungeonService.GetDungeonOptionAsync(id, token);
        renamed.DungeonName.ShouldBe(newName);
    }
}