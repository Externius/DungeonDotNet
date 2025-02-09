using RDMG.Core.Abstractions.Repository;
using RDMG.Core.Abstractions.Services;
using RDMG.Core.Domain;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RDMG.Core.Tests.OptionServiceTests;

public class List(TestFixture fixture) : IClassFixture<TestFixture>
{
    private readonly IOptionService _optionService = fixture.OptionService;
    private readonly IOptionRepository _optionRepository = fixture.OptionRepository;

    [Fact]
    public async Task ListOptionsAsync_WithNoFilter_ReturnsAllOptions()
    {
        var expectedCount =
            (await _optionRepository.ListAsync(cancellationToken: TestContext.Current.CancellationToken)).Count();
        var result = await _optionService.ListOptionsAsync(cancellationToken: TestContext.Current.CancellationToken);
        result.Count().ShouldBe(expectedCount);
    }

    public static TheoryData<OptionKey> GetOptionKeys()
    {
        var result = new TheoryData<OptionKey>();
        foreach (var key in Enum.GetValues<OptionKey>())
        {
            result.Add(key);
        }

        return result;
    }

    [Theory]
    [MemberData(nameof(GetOptionKeys), MemberType = typeof(List))]
    public async Task ListOptionsAsync_WithFilter_ReturnsFilteredOptions(OptionKey filter)
    {
        var expectedCount = (await _optionRepository.ListAsync(filter, TestContext.Current.CancellationToken)).Count();
        var result = await _optionService.ListOptionsAsync(filter, TestContext.Current.CancellationToken);
        result.Count().ShouldBe(expectedCount);
    }
}