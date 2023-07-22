using RDMG.Core.Abstractions.Services;
using Shouldly;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RDMG.Core.Tests.DungeonServiceTests;

public class Get
{
    [Fact]
    public async Task CanGetAllDungeonOptions()
    {
        using var env = new TestEnvironment();
        var service = env.GetService<IDungeonService>();
        var source = new CancellationTokenSource();
        var token = source.Token;
        var result = (await service.GetAllDungeonOptionsAsync(token)).ToList();
        result.ShouldNotBeEmpty();
        result.Count.ShouldBe(2);
    }

    [Fact]
    public async Task CanGetAllDungeonOptionsForUser()
    {
        using var env = new TestEnvironment();
        var service = env.GetService<IDungeonService>();
        var source = new CancellationTokenSource();
        var token = source.Token;
        var result = await service.GetAllDungeonOptionsForUserAsync(2, token);
        result.ShouldBeEmpty();
    }

    [Fact]
    public async Task CanGetDungeonOptionByName()
    {
        using var env = new TestEnvironment();
        var service = env.GetService<IDungeonService>();
        var source = new CancellationTokenSource();
        var token = source.Token;
        var option = (await service.GetAllDungeonOptionsAsync(token)).First();
        var result = await service.GetDungeonOptionByNameAsync(option.DungeonName, option.UserId, token);
        result.ShouldNotBeNull();
        result.ShouldBeEquivalentTo(option);
    }

    [Fact]
    public async Task CanGetDungeon()
    {
        using var env = new TestEnvironment();
        var service = env.GetService<IDungeonService>();
        var source = new CancellationTokenSource();
        var token = source.Token;
        const int id = 1;
        var result = await service.GetDungeonAsync(id, token);
        result.Id.ShouldBe(id);
    }
}