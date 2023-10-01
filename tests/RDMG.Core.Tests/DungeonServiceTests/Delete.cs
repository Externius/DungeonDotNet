using Shouldly;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RDMG.Core.Tests.DungeonServiceTests;

public class Delete : DungeonServiceTestBase
{
    [Fact]
    public async Task DeleteDungeonOptionAsync_WithValidId_ReturnsTrue()
    {
        const int toDeleteId = 1;
        var source = new CancellationTokenSource();
        var token = source.Token;
        var result = await DungeonService.DeleteDungeonOptionAsync(toDeleteId, token);
        result.ShouldBeTrue();

        var list = await DungeonService.GetAllDungeonOptionsAsync(token);
        list.Any(o => o.Id == toDeleteId).ShouldBeFalse();
    }

    [Fact]
    public async Task DeleteDungeonAsync_WithValidId_ReturnsTrue()
    {
        const int toDeleteId = 1;
        var source = new CancellationTokenSource();
        var token = source.Token;
        var result = await DungeonService.DeleteDungeonAsync(toDeleteId, token);
        result.ShouldBeTrue();
    }
}