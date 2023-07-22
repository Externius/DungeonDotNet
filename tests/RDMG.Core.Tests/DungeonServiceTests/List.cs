﻿using RDMG.Core.Abstractions.Services;
using Shouldly;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RDMG.Core.Tests.DungeonServiceTests;

public class List
{
    [Fact]
    public async Task CanListUserDungeons()
    {
        using var env = new TestEnvironment();
        var service = env.GetService<IDungeonService>();
        var source = new CancellationTokenSource();
        var token = source.Token;
        var result = await service.ListUserDungeonsAsync(1, token);
        result.Count.ShouldBe(2);
    }

    [Fact]
    public async Task CanListDungeonsByName()
    {
        using var env = new TestEnvironment();
        var service = env.GetService<IDungeonService>();
        var source = new CancellationTokenSource();
        var token = source.Token;
        var dungeonName = (await service.GetAllDungeonOptionsAsync(token)).First().DungeonName;
        var result = await service.ListUserDungeonsByNameAsync(dungeonName, 1, token);
        result.Count.ShouldBe(1);
    }
}