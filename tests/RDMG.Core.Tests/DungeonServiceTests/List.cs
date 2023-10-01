using Shouldly;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RDMG.Core.Tests.DungeonServiceTests;

public class List : DungeonServiceTestBase
{
    private const int UserId = 1;

    [Fact]
    public async Task ListUserDungeonsAsync_WithValidUserIdWithDungeons_ReturnsDungeonModelList()
    {
        var source = new CancellationTokenSource();
        var token = source.Token;
        const int expectedCount = 2;
        var result = await DungeonService.ListUserDungeonsAsync(UserId, token);
        result.Count.ShouldBe(expectedCount);
    }

    [Fact]
    public async Task ListUserDungeonsByNameAsync_WithValidNameAndUserId_ReturnsDungeonModelList()
    {
        var source = new CancellationTokenSource();
        var token = source.Token;
        const int expectedCount = 1;
        var dungeonName = (await DungeonService.GetAllDungeonOptionsAsync(token)).First().DungeonName;
        var result = await DungeonService.ListUserDungeonsByNameAsync(dungeonName, UserId, token);
        result.Count.ShouldBe(expectedCount);
    }
}