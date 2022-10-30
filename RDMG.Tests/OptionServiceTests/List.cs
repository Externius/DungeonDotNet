using RDMG.Core.Abstractions.Services;
using Shouldly;
using System.Linq;
using System.Threading;
using Xunit;

namespace RDMG.Tests.OptionServiceTests;

public class List
{

    [Fact]
    public async void CanListOptions()
    {
        using var env = new TestEnvironment();
        var service = env.GetService<IOptionService>();
        var source = new CancellationTokenSource();
        var token = source.Token;
        var result = await service.ListOptionsAsync(token);
        result.Any(om => om.Key == Core.Domain.OptionKey.Size).ShouldBeTrue();
    }
}