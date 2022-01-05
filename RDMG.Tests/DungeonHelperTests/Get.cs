using RDMG.Core.Abstractions.Dungeon;
using RDMG.Core.Abstractions.Dungeon.Models;
using RDMG.Core.Domain;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RDMG.Tests.DungeonHelperTests
{
    public class Get
    {
        [Fact]
        public void CanGetRandomInt()
        {
            using var env = new TestEnvironment();
            var service = env.GetService<IDungeonHelper>();
            var result = service.GetRandomInt(1, 10);

            result.ShouldBeGreaterThan(0);
            result.ShouldBeLessThan(10);
        }

        [Fact]
        public void CanGetNCDoor()
        {
            using var env = new TestEnvironment();
            var service = env.GetService<IDungeonHelper>();

            var currentDoors = new List<DungeonTile>(){
                    new DungeonTile(2,2,2,2,50,50,Textures.NO_CORRIDOR_DOOR),
                    new DungeonTile(5,5,2,2,50,50,Textures.NO_CORRIDOR_DOOR_TRAPPED),
                    new DungeonTile(5,2,2,2,50,50,Textures.NO_CORRIDOR_DOOR_LOCKED)
                };

            var result = new string[currentDoors.Count];
            for (int i = 0; i < currentDoors.Count; i++)
            {
                result[i] = service.GetNCDoor(currentDoors[i]);
            }

            result.Any(s => !string.IsNullOrWhiteSpace(s)).ShouldBeTrue();
        }

        [Fact]
        public void CanGetNCDoorDescription()
        {
            using var env = new TestEnvironment();
            var service = env.GetService<IDungeonHelper>();
            var dungeonNoCorridor = env.GetNCDungeon();
            dungeonNoCorridor.Init();
            dungeonNoCorridor.AddFirstRoom();
            var list = dungeonNoCorridor.DungeonTiles.SelectMany(T => T).ToList();
            var match = list.Where(x => x.Texture == Textures.ROOM);
            var result = service.GetNCDoorDescription(dungeonNoCorridor.DungeonTiles, match.ToList());
            result.ShouldNotBeNull();
        }

        [Fact]
        public void CanGetTreasure()
        {
            using var env = new TestEnvironment();
            var service = env.GetService<IDungeonHelper>();
            var dungeon = env.GetDungeon();
            var result = new string[20];
            for (var i = 0; i < 20; i++)
            {
                result[i] = service.GetTreasure();
            }
            result.Any(s => !string.IsNullOrWhiteSpace(s)).ShouldBeTrue();
        }

        [Fact]
        public void CanGetMonster()
        {
            using var env = new TestEnvironment();
            var service = env.GetService<IDungeonHelper>();
            var dungeon = env.GetDungeon();
            var result = new string[20];
            for (var i = 0; i < 20; i++)
            {
                result[i] = service.GetMonster();
            }
            result.Any(s => !string.IsNullOrWhiteSpace(s)).ShouldBeTrue();
        }

        [Fact]
        public void CanGetCurrentTrap()
        {
            using var env = new TestEnvironment();
            var service = env.GetService<IDungeonHelper>();
            var dungeon = env.GetDungeon();
            var result = new string[20];
            for (var i = 0; i < 20; i++)
            {
                result[i] = service.GetCurrentTrap(false);
            }
            result.Any(s => !string.IsNullOrWhiteSpace(s)).ShouldBeTrue();
        }

        [Fact]
        public void CanGetRoamingMonster()
        {
            using var env = new TestEnvironment();
            var service = env.GetService<IDungeonHelper>();
            var dungeon = env.GetDungeon();
            var result = new string[20];
            for (var i = 0; i < 20; i++)
            {
                result[i] = service.GetRoamingMonster();
            }
            result.Any(s => !string.IsNullOrWhiteSpace(s)).ShouldBeTrue();
        }
    }
}
