using RDMG.Core.Abstractions.Services;
using RDMG.Core.Abstractions.Services.Exceptions;
using RDMG.Core.Abstractions.Services.Models;
using RDMG.Core.Domain;
using RDMG.Infrastructure.Data;
using RDMG.Resources;
using Shouldly;
using Xunit;

namespace RDMG.Core.Tests.UserServiceTests;

public class Create(TestFixture fixture) : IClassFixture<TestFixture>
{
    private readonly IUserService _userService = fixture.UserService;

    [Fact]
    public async Task CreateAsync_WithValidModel_CreatesUser()
    {
        var model = new UserModel
        {
            Username = "ddd",
            Password = "asdasdada+mnn!",
            Email = "dasd@test.com",
            FirstName = "John",
            LastName = "Doe",
            Role = nameof(Role.Admin)
        };
        var result = await _userService.CreateAsync(model, cancellationToken: TestContext.Current.CancellationToken);
        result.ShouldBeGreaterThan(AppDbContextInitializer.TestDeletedUserId);
    }

    [Fact]
    public async Task CreateAsync_WithInValidModel_ThrowsServiceAggregateException()
    {
        var model = new UserModel();
        var expectedErrors = new List<string>
        {
            string.Format(Error.RequiredValidation, nameof(model.Username)),
            string.Format(Error.RequiredValidation, nameof(model.FirstName)),
            string.Format(Error.RequiredValidation, nameof(model.LastName)),
            string.Format(Error.RequiredValidation, nameof(model.Email)),
            string.Format(Error.RequiredValidation, nameof(model.Role)),
            Error.PasswordLength
        };

        var act = async () =>
        {
            await _userService.CreateAsync(model, cancellationToken: TestContext.Current.CancellationToken);
        };

        var result = await act.ShouldThrowAsync<ServiceAggregateException>();
        result.GetInnerExceptions()
            .Select(se => se.Message)
            .OrderBy(s => s)
            .SequenceEqual(expectedErrors.OrderBy(s => s))
            .ShouldBeTrue();
    }
}