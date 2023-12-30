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
        var expectedCount = (await _optionRepository.ListAsync()).Count();
        var result = await _optionService.ListOptionsAsync();
        result.Count().ShouldBe(expectedCount);
    }

    public static TheoryData<OptionKey> GetOptionKeys()
    {
        var result = new TheoryData<OptionKey>();
        foreach (OptionKey key in Enum.GetValues(typeof(OptionKey)))
        {
            result.Add(key);
        }
        return result;
    }

    [Theory]
    [MemberData(nameof(GetOptionKeys), MemberType = typeof(List))]
    public async Task ListOptionsAsync_WithFilter_ReturnsFilteredOptions(OptionKey filter)
    {
        var expectedCount = (await _optionRepository.ListAsync(filter)).Count();
        var result = await _optionService.ListOptionsAsync(filter);
        result.Count().ShouldBe(expectedCount);
    }
}