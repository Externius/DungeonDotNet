using RDMG.Core.Abstractions.Services;
using RDMG.Infrastructure.Data;
using Shouldly;
using Xunit;

namespace RDMG.Core.Tests.UserServiceTests;

public class Delete(TestFixture fixture) : IClassFixture<TestFixture>
{
    private readonly IUserService _userService = fixture.UserService;

    [Fact]
    public async Task DeleteAsync_WithValidId_ReturnsTrue()
    {
        var result = await _userService.DeleteAsync(AppDbContextInitializer.TestAdminUserId,
            cancellationToken: TestContext.Current.CancellationToken);
        result.ShouldBe(true);
    }

    [Fact]
    public async Task DeleteAsync_WithInValidId_ReturnsFalse()
    {
        var result = await _userService.DeleteAsync(AppDbContextInitializer.TestNotExistingUserId,
            cancellationToken: TestContext.Current.CancellationToken);
        result.ShouldBe(false);
    }
}