using RDMG.Core.Abstractions.Services;
using RDMG.Infrastructure.Data;
using Shouldly;
using Xunit;

namespace RDMG.Core.Tests.UserServiceTests;

public class Get(TestFixture fixture) : IClassFixture<TestFixture>
{
    private readonly IUserService _userService = fixture.UserService;

    [Theory]
    [InlineData(AppDbContextInitializer.TestAdminUserId)]
    [InlineData(AppDbContextInitializer.TestUserId)]
    [InlineData(AppDbContextInitializer.TestDeletedUserId)]
    public async Task GetAsync_WithValidId_ReturnsUser(int id)
    {
        var result = await _userService.GetAsync(id, TestContext.Current.CancellationToken);
        result.ShouldNotBeNull();
        result.Id.ShouldBe(id);
    }
}