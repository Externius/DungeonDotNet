using Microsoft.Extensions.Options;
using RDMG.Core.Abstractions.Configuration;
using RDMG.Core.Abstractions.Services;
using RDMG.Core.Abstractions.Services.Exceptions;
using RDMG.Core.Abstractions.Services.Models;
using RDMG.Infrastructure.Data;
using RDMG.Resources;
using Shouldly;
using Xunit;

namespace RDMG.Core.Tests.UserServiceTests;

public class ChangePassword(TestFixture fixture) : IClassFixture<TestFixture>
{
    private readonly IUserService _userService = fixture.UserService;
    private readonly IOptions<AppConfig> _config = fixture.Config;
    private const string Password = "SOMENEWPASSWORD123";

    [Fact]
    public async Task ChangePasswordAsync_WithValidInput_ChangesPassword()
    {
        var model = new ChangePasswordModel
        {
            Id = AppDbContextInitializer.TestUserId,
            CurrentPassword = _config.Value.DefaultUserPassword,
            NewPassword = Password
        };
        var oldUserModel = await _userService.GetAsync(model.Id, TestContext.Current.CancellationToken);
        var act = async () => { await _userService.ChangePasswordAsync(model, TestContext.Current.CancellationToken); };

        await act.ShouldNotThrowAsync();
        var newUserModel = await _userService.GetAsync(model.Id, TestContext.Current.CancellationToken);
        newUserModel.Password.ShouldNotBe(oldUserModel.Password);
    }

    public static TheoryData<ChangePasswordModel, string> GetModelsWithErrors()
    {
        return
        [
            (new ChangePasswordModel
            {
                NewPassword = "length"
            }, Error.PasswordLength),
            (new ChangePasswordModel
            {
                NewPassword = Password,
                Id = AppDbContextInitializer.TestNotExistingUserId,
            }, Error.NotFound),
            (new ChangePasswordModel
            {
                NewPassword = Password,
                Id = AppDbContextInitializer.TestUserId,
                CurrentPassword = "wrong"
            }, Error.PasswordMissMatch)
        ];
    }

    [Theory]
    [MemberData(nameof(GetModelsWithErrors), MemberType = typeof(ChangePassword))]
    public async Task ChangePasswordAsync_WithInValidModel_ThrowsServiceException(ChangePasswordModel model,
        string expectedError)
    {
        var act = async () =>
        {
            await _userService.ChangePasswordAsync(model, cancellationToken: TestContext.Current.CancellationToken);
        };

        var result = await act.ShouldThrowAsync<ServiceException>();
        result.Message.ShouldBe(expectedError);
    }
}