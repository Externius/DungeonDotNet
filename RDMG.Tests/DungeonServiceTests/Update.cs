﻿using RDMG.Core.Abstractions.Services;
using Shouldly;
using System.Threading;
using Xunit;

namespace RDMG.Tests.DungeonServiceTests
{
    public class Update
    {
        [Fact]
        public async void CanUpdateDungeon()
        {
            using var env = new TestEnvironment();
            var service = env.GetService<IDungeonService>();
            var source = new CancellationTokenSource();
            var token = source.Token;
            var dungeonId = 1;
            var oldDungeon = await service.GetDungeonAsync(dungeonId, token);
            var optionmodel = await service.GetDungeonOptionAsync(oldDungeon.DungeonOptionId, token);
            optionmodel.Corridor = false;
            var newDungeon = await service.GenerateDungeonAsync(optionmodel);
            oldDungeon.DungeonTiles = newDungeon.DungeonTiles;
            oldDungeon.TrapDescription = newDungeon.TrapDescription;
            oldDungeon.RoomDescription = newDungeon.RoomDescription;
            oldDungeon.RoamingMonsterDescription = newDungeon.RoamingMonsterDescription;
            await service.UpdateDungeonAsync(oldDungeon, token);

            var result = await service.GetDungeonAsync(dungeonId, token);
            result.RoamingMonsterDescription.ShouldBe("[]");
        }
    }
}