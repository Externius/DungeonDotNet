using RDMG.Core.Abstractions.Services;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RDMG.Core.Tests.DungeonServiceTests;

public class List(TestFixture fixture) : IClassFixture<TestFixture>
{
    private const int UserId = 1;
    private readonly IDungeonService _dungeonService = fixture.DungeonService;

    [Fact]
    public async Task ListUserDungeonsAsync_WithValidUserIdWithDungeons_ReturnsDungeonModelList()
    {
        const int expectedCount = 2;
        var result = await _dungeonService.ListUserDungeonsAsync(UserId, TestContext.Current.CancellationToken);
        result.Count().ShouldBe(expectedCount);
    }

    [Fact]
    public async Task ListUserDungeonsByNameAsync_WithValidNameAndUserId_ReturnsDungeonModelList()
    {
        const int expectedCount = 1;
        var dungeonName = (await _dungeonService.GetAllDungeonOptionsAsync(TestContext.Current.CancellationToken))
            .First().DungeonName;
        var result =
            await _dungeonService.ListUserDungeonsByNameAsync(dungeonName, UserId,
                TestContext.Current.CancellationToken);
        result.Count().ShouldBe(expectedCount);
    }
}