using RDMG.Core.Abstractions.Repository;
using RDMG.Core.Abstractions.Services;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RDMG.Core.Tests.OptionServiceTests;

public class List : IClassFixture<TestFixture>
{
    private readonly IOptionService _optionService;
    private readonly IOptionRepository _optionRepository;
    public List(TestFixture fixture)
    {
        _optionService = fixture.OptionService;
        _optionRepository = fixture.OptionRepository;
    }

    [Fact]
    public async Task ListOptionsAsync_WithNoFilter_ReturnsAllOptions()
    {
        var expectedCount = (await _optionRepository.ListAsync()).Count();
        var result = await _optionService.ListOptionsAsync();
        result.Count().ShouldBe(expectedCount);
    }

    public static IEnumerable<object[]> GetOptionKeys()
    {
        return from object value in Enum.GetValues(typeof(Domain.OptionKey)) select new[] { value };
    }

    [Theory]
    [MemberData(nameof(GetOptionKeys))]
    public async Task ListOptionsAsync_WithFilter_ReturnsFilteredOptions(Domain.OptionKey filter)
    {
        var expectedCount = (await _optionRepository.ListAsync(filter)).Count();
        var result = await _optionService.ListOptionsAsync(filter);
        result.Count().ShouldBe(expectedCount);
    }
}