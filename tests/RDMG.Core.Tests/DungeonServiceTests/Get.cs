using RDMG.Core.Abstractions.Services;
using Shouldly;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RDMG.Core.Tests.DungeonServiceTests;

public class Get(TestFixture fixture) : IClassFixture<TestFixture>
{
    private readonly IDungeonService _dungeonService = fixture.DungeonService;

    [Fact]
    public async Task GetAllDungeonOptionsAsync_ReturnsDungeonOptionModelList()
    {
        using var source = new CancellationTokenSource();
        var token = source.Token;
        const int expectedCount = 4;
        var result = (await _dungeonService.GetAllDungeonOptionsAsync(token)).ToList();
        result.ShouldNotBeEmpty();
        result.Count.ShouldBe(expectedCount);
    }

    [Fact]
    public async Task GetAllDungeonOptionsForUserAsync_WithNotExistingUserId_ReturnsEmptyList()
    {
        using var source = new CancellationTokenSource();
        var token = source.Token;
        const int userId = 3;
        var result = await _dungeonService.GetAllDungeonOptionsForUserAsync(userId, token);
        result.ShouldBeEmpty();
    }

    [Fact]
    public async Task GetDungeonOptionByNameAsync_WithValidName_ReturnsDungeonOptionModel()
    {
        using var source = new CancellationTokenSource();
        var token = source.Token;
        var option = (await _dungeonService.GetAllDungeonOptionsAsync(token)).First();
        var result = await _dungeonService.GetDungeonOptionByNameAsync(option.DungeonName, option.UserId, token);
        result.ShouldNotBeNull();
        result.ShouldBeEquivalentTo(option);
    }

    [Fact]
    public async Task GetDungeonAsync_WithValidDungeonId_ReturnsDungeonModel()
    {
        using var source = new CancellationTokenSource();
        var token = source.Token;
        const int id = 1;
        var result = await _dungeonService.GetDungeonAsync(id, token);
        result.Id.ShouldBe(id);
    }
}