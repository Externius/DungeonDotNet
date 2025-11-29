using RDMG.Core.Abstractions.Services;
using Shouldly;
using Xunit;

namespace RDMG.Core.Tests.UserServiceTests;

public class List(TestFixture fixture) : IClassFixture<TestFixture>
{
    private readonly IUserService _userService = fixture.UserService;

    [Theory]
    [InlineData(false, 2)]
    [InlineData(true, 1)]
    public async Task ListAsync_WithDeletedFlag_ReturnsAllUsers(bool deleted, int expectedCount)
    {
        var result = await _userService.ListAsync(deleted, cancellationToken: TestContext.Current.CancellationToken);
        result.Length.ShouldBe(expectedCount);
    }
}