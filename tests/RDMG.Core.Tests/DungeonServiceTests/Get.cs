using Shouldly;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RDMG.Core.Tests.DungeonServiceTests;

public class Get : DungeonServiceTestBase
{
    [Fact]
    public async Task GetAllDungeonOptionsAsync_ReturnsDungeonOptionModelList()
    {
        var source = new CancellationTokenSource();
        var token = source.Token;
        const int expectedCount = 2;
        var result = (await DungeonService.GetAllDungeonOptionsAsync(token)).ToList();
        result.ShouldNotBeEmpty();
        result.Count.ShouldBe(expectedCount);
    }

    [Fact]
    public async Task GetAllDungeonOptionsForUserAsync_WithValidUserIdWithoutGeneratedDungeons_ReturnsEmptyList()
    {
        var source = new CancellationTokenSource();
        var token = source.Token;
        const int userId = 2;
        var result = await DungeonService.GetAllDungeonOptionsForUserAsync(userId, token);
        result.ShouldBeEmpty();
    }

    [Fact]
    public async Task GetDungeonOptionByNameAsync_WithValidName_ReturnsDungeonOptionModel()
    {
        var source = new CancellationTokenSource();
        var token = source.Token;
        var option = (await DungeonService.GetAllDungeonOptionsAsync(token)).First();
        var result = await DungeonService.GetDungeonOptionByNameAsync(option.DungeonName, option.UserId, token);
        result.ShouldNotBeNull();
        result.ShouldBeEquivalentTo(option);
    }

    [Fact]
    public async Task GetDungeonAsync_WithValidDungeonId_ReturnsDungeonModel()
    {
        var source = new CancellationTokenSource();
        var token = source.Token;
        const int id = 1;
        var result = await DungeonService.GetDungeonAsync(id, token);
        result.Id.ShouldBe(id);
    }
}