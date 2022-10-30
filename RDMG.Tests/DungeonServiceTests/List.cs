using RDMG.Core.Abstractions.Services;
using Shouldly;
using System.Linq;
using System.Threading;
using Xunit;

namespace RDMG.Tests.DungeonServiceTests;

public class List
{
    [Fact]
    public async void CanListUserDungeons()
    {
        using var env = new TestEnvironment();
        var service = env.GetService<IDungeonService>();
        var source = new CancellationTokenSource();
        var token = source.Token;
        var result = await service.ListUserDungeonsAsync(1, token);
        result.Count.ShouldBe(2);
    }

    [Fact]
    public async void CanListDungeonsByName()
    {
        using var env = new TestEnvironment();
        var service = env.GetService<IDungeonService>();
        var source = new CancellationTokenSource();
        var token = source.Token;
        var dungeonName = (await service.GetAllDungeonOptionsAsync(token)).First().DungeonName;
        var result = await service.ListUserDungeonsByNameAsync(dungeonName, 1, token);
        result.Count.ShouldBe(1);
    }
}