using RDMG.Core.Abstractions.Services;
using RDMG.Infrastructure.Data;
using Shouldly;
using Xunit;

namespace RDMG.Core.Tests.UserServiceTests;

public class Restore(TestFixture fixture) : IClassFixture<TestFixture>
{
    private readonly IUserService _userService = fixture.UserService;

    [Fact]
    public async Task RestoreAsync_WithValidId_ReturnsTrue()
    {
        var result = await _userService.RestoreAsync(AppDbContextInitializer.TestDeletedUserId,
            cancellationToken: TestContext.Current.CancellationToken);
        result.ShouldBe(true);
    }

    [Fact]
    public async Task RestoreAsync_WithInValidId_ReturnsFalse()
    {
        var result = await _userService.RestoreAsync(AppDbContextInitializer.TestNotExistingUserId,
            cancellationToken: TestContext.Current.CancellationToken);
        result.ShouldBe(false);
    }
}