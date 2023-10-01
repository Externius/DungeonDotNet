using RDMG.Core.Abstractions.Repository;
using RDMG.Core.Abstractions.Services;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RDMG.Core.Tests.OptionServiceTests;

public class List
{
    private readonly IOptionService _optionService;
    private readonly IOptionRepository _optionRepository;
    public List()
    {
        var env = new TestEnvironment();
        _optionService = env.GetService<IOptionService>();
        _optionRepository = env.GetService<IOptionRepository>();
    }

    [Fact]
    public async void ListOptionsAsync_WithNoFilter_ReturnsAllOptions()
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
    public async void ListOptionsAsync_WithFilter_ReturnsFilteredOptions(Domain.OptionKey filter)
    {
        var expectedCount = (await _optionRepository.ListAsync(filter)).Count();
        var result = await _optionService.ListOptionsAsync(filter);
        result.Count().ShouldBe(expectedCount);
    }
}