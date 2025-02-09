using RDMG.Core.Abstractions.Services;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RDMG.Core.Tests.DungeonServiceTests;

public class Delete(TestFixture fixture) : IClassFixture<TestFixture>
{
    private readonly IDungeonService _dungeonService = fixture.DungeonService;

    [Fact]
    public async Task DeleteDungeonOptionAsync_WithValidId_ReturnsTrue()
    {
        const int toDeleteId = 1;
        var result = await _dungeonService.DeleteDungeonOptionAsync(toDeleteId, TestContext.Current.CancellationToken);
        result.ShouldBeTrue();

        var list = await _dungeonService.GetAllDungeonOptionsAsync(TestContext.Current.CancellationToken);
        list.Any(o => o.Id == toDeleteId).ShouldBeFalse();
    }

    [Fact]
    public async Task DeleteDungeonAsync_WithValidId_ReturnsTrue()
    {
        const int toDeleteId = 2;
        var result = await _dungeonService.DeleteDungeonAsync(toDeleteId, TestContext.Current.CancellationToken);
        result.ShouldBeTrue();
    }
}