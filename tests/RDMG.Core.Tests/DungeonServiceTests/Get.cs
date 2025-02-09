using RDMG.Core.Abstractions.Services;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RDMG.Core.Tests.DungeonServiceTests;

public class Get(TestFixture fixture) : IClassFixture<TestFixture>
{
    private readonly IDungeonService _dungeonService = fixture.DungeonService;

    [Fact]
    public async Task GetAllDungeonOptionsAsync_ReturnsDungeonOptionModelList()
    {
        const int expectedCount = 4;
        var result = (await _dungeonService.GetAllDungeonOptionsAsync(TestContext.Current.CancellationToken)).ToList();
        result.ShouldNotBeEmpty();
        result.Count.ShouldBe(expectedCount);
    }

    [Fact]
    public async Task GetAllDungeonOptionsForUserAsync_WithNotExistingUserId_ReturnsEmptyList()
    {
        const int userId = 3;
        var result =
            await _dungeonService.GetAllDungeonOptionsForUserAsync(userId, TestContext.Current.CancellationToken);
        result.ShouldBeEmpty();
    }

    [Fact]
    public async Task GetDungeonOptionByNameAsync_WithValidName_ReturnsDungeonOptionModel()
    {
        var option = (await _dungeonService.GetAllDungeonOptionsAsync(TestContext.Current.CancellationToken)).First();
        var result = await _dungeonService.GetDungeonOptionByNameAsync(option.DungeonName, option.UserId,
            TestContext.Current.CancellationToken);
        result.ShouldNotBeNull();
        result.ShouldBeEquivalentTo(option);
    }

    [Fact]
    public async Task GetDungeonAsync_WithValidDungeonId_ReturnsDungeonModel()
    {
        const int id = 1;
        var result = await _dungeonService.GetDungeonAsync(id, TestContext.Current.CancellationToken);
        result.Id.ShouldBe(id);
    }
}