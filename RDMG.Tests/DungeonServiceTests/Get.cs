using RDMG.Core.Abstractions.Services;
using Shouldly;
using System.Linq;
using System.Threading;
using Xunit;

namespace RDMG.Tests.DungeonServiceTests
{
    public class Get
    {
        [Fact]
        public async void CanGetAllDungeonOptions()
        {
            using var env = new TestEnvironment();
            var service = env.GetService<IDungeonService>();
            var source = new CancellationTokenSource();
            var token = source.Token;
            var result = await service.GetAllDungeonOptionsAsync(token);
            result.ShouldNotBeEmpty();
            result.Count().ShouldBe(2);
        }

        [Fact]
        public async void CanGetAllDungeonOptionsForUser()
        {
            using var env = new TestEnvironment();
            var service = env.GetService<IDungeonService>();
            var source = new CancellationTokenSource();
            var token = source.Token;
            var result = await service.GetAllDungeonOptionsForUserAsync(2, token);
            result.ShouldBeEmpty();
        }

        [Fact]
        public async void CanGetDungeonOptionByName()
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
        public async void CanGetDungeon()
        {
            using var env = new TestEnvironment();
            var service = env.GetService<IDungeonService>();
            var source = new CancellationTokenSource();
            var token = source.Token;
            var id = 1;
            var result = await service.GetDungeonAsync(id, token);
            result.Id.ShouldBe(id);
        }
    }
}
