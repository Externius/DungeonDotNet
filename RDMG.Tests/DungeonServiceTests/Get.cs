using RDMG.Core.Abstractions.Services;
using Shouldly;
using System.Threading;
using Xunit;

namespace RDMG.Tests.DungeonServiceTests
{
    public class Get
    {
        [Fact]
        public async void CanGetAllOptions()
        {
            using var env = new TestEnvironment();
            var service = env.GetService<IDungeonService>();
            var source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            var result = await service.GetAllOptionsAsync(token);
            result.ShouldNotBeEmpty();
        }
        [Fact]
        public async void CanGetUserOptionsWithSavedDungeons()
        {
            using var env = new TestEnvironment();
            var service = env.GetService<IDungeonService>();
            var source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            var result = await service.GetUserOptionsWithSavedDungeonsAsync(1, token);
            result.ShouldNotBeEmpty();
        }
    }
}
