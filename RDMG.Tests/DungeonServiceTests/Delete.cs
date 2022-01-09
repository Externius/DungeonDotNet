using RDMG.Core.Abstractions.Services;
using Shouldly;
using System.Linq;
using System.Threading;
using Xunit;

namespace RDMG.Tests.DungeonServiceTests
{
    public class Delete
    {
        [Fact]
        public async void CanDeleteDungeonOption()
        {
            using var env = new TestEnvironment();
            var service = env.GetService<IDungeonService>();
            var toDeleteId = 1;
            var source = new CancellationTokenSource();
            var token = source.Token;
            var result = await service.DeleteDungeonOptionAsync(toDeleteId, token);
            result.ShouldBeTrue();

            var list = await service.GetAllDungeonOptionsAsync(token);
            list.Any(o => o.Id == toDeleteId).ShouldBeFalse();
        }

        [Fact]
        public async void CanDeleteDungeon()
        {
            using var env = new TestEnvironment();
            var service = env.GetService<IDungeonService>();
            var toDeleteId = 1;
            var source = new CancellationTokenSource();
            var token = source.Token;
            var result = await service.DeleteDungeonAsync(toDeleteId, token);
            result.ShouldBeTrue();
        }
    }
}
