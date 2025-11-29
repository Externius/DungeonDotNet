using RDMG.Core.Abstractions.Services;
using RDMG.Core.Abstractions.Services.Exceptions;
using RDMG.Core.Abstractions.Services.Models;
using RDMG.Infrastructure.Data;
using RDMG.Resources;
using Shouldly;
using Xunit;

namespace RDMG.Core.Tests.UserServiceTests;

public class Update(TestFixture fixture) : IClassFixture<TestFixture>
{
    private readonly IUserService _userService = fixture.UserService;
    private const string ModifiedText = "Modified";

    [Fact]
    public async Task UpdateAsync_WithValidModel_UpdatesUser()
    {
        var adminUser = await _userService.GetAsync(AppDbContextInitializer.TestAdminUserId,
            cancellationToken: TestContext.Current.CancellationToken);
        adminUser.FirstName = ModifiedText;
        await _userService.UpdateAsync(adminUser, cancellationToken: TestContext.Current.CancellationToken);
        var result =
            await _userService.GetAsync(adminUser.Id, cancellationToken: TestContext.Current.CancellationToken);
        result.FirstName.ShouldBe(adminUser.FirstName);
    }

    [Fact]
    public async Task UpdateAsync_WithInValidModel_ThrowsServiceAggregateException()
    {
        var model = new UserModel();
        var expectedErrors = new List<string>
        {
            string.Format(Error.RequiredValidation, nameof(model.FirstName)),
            string.Format(Error.RequiredValidation, nameof(model.LastName)),
            string.Format(Error.RequiredValidation, nameof(model.Email)),
            string.Format(Error.RequiredValidation, nameof(model.Role))
        };

        var act = async () =>
        {
            await _userService.UpdateAsync(model, cancellationToken: TestContext.Current.CancellationToken);
        };

        var result = await act.ShouldThrowAsync<ServiceAggregateException>();
        result.GetInnerExceptions()
            .Select(se => se.Message)
            .OrderBy(s => s)
            .SequenceEqual(expectedErrors.OrderBy(s => s))
            .ShouldBeTrue();
    }
}