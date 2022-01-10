using RDMG.Core.Abstractions.Services;
using RDMG.Core.Abstractions.Services.Models;
using RDMG.Core.Domain;
using RDMG.Core.Helpers;
using RDMG.Infrastructure;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RDMG.Seed
{
    public class ContextSeedData
    {
        private readonly Context _context;
        private readonly IDungeonService _dungeonService;

        public ContextSeedData(IDungeonService dungeonService, Context context)
        {
            _context = context;
            _dungeonService = dungeonService;
        }

        public async Task SeedDataAsync()
        {
            if (!_context.Users.Any())
            {
                var source = new CancellationTokenSource();
                CancellationToken token = source.Token;
                await SeedUsers(token);
                await SeedDungeons(token);
            }
        }

        private async Task SeedDungeons(CancellationToken token)
        {
            var dungeonOption = new DungeonOption()
            {
                UserId = 1,
                DungeonName = "Test1",
                Created = DateTime.UtcNow,
                ItemsRarity = 1,
                DeadEnd = true,
                DungeonDifficulty = 1,
                DungeonSize = 20,
                MonsterType = "any",
                PartyLevel = 4,
                PartySize = 4,
                TrapPercent = 20,
                RoamingPercent = 10,
                TreasureValue = 1,
                RoomDensity = 10,
                RoomSize = 20,
                Corridor = true
            };

            _context.DungeonOptions.Add(dungeonOption);
            await _context.SaveChangesAsync(token);

            var model = new DungeonOptionModel()
            {
                DungeonName = dungeonOption.DungeonName,
                Created = dungeonOption.Created,
                ItemsRarity = dungeonOption.ItemsRarity,
                DeadEnd = dungeonOption.DeadEnd,
                DungeonDifficulty = dungeonOption.DungeonDifficulty,
                DungeonSize = dungeonOption.DungeonSize,
                MonsterType = dungeonOption.MonsterType,
                PartyLevel = dungeonOption.PartyLevel,
                PartySize = dungeonOption.PartySize,
                TrapPercent = dungeonOption.TrapPercent,
                RoamingPercent = dungeonOption.RoamingPercent,
                TreasureValue = dungeonOption.TreasureValue,
                RoomDensity = dungeonOption.RoomDensity,
                RoomSize = dungeonOption.RoomSize,
                Corridor = dungeonOption.Corridor,
                Id = dungeonOption.Id,
                UserId = dungeonOption.UserId
            };

            var sd = await _dungeonService.GenerateDungeonAsync(model);
            sd.Level = 1;
            await _dungeonService.AddDungeonAsync(sd, token);

            dungeonOption = new DungeonOption()
            {
                UserId = 1,
                DungeonName = "Test2",
                Created = DateTime.UtcNow,
                ItemsRarity = 1,
                DeadEnd = true,
                DungeonDifficulty = 1,
                DungeonSize = 25,
                MonsterType = "any",
                PartyLevel = 4,
                PartySize = 4,
                TrapPercent = 20,
                RoamingPercent = 0,
                TreasureValue = 1,
                RoomDensity = 10,
                RoomSize = 20,
                Corridor = false
            };
            _context.DungeonOptions.Add(dungeonOption);
            await _context.SaveChangesAsync(token);

            model = new DungeonOptionModel()
            {
                DungeonName = dungeonOption.DungeonName,
                Created = dungeonOption.Created,
                ItemsRarity = dungeonOption.ItemsRarity,
                DeadEnd = dungeonOption.DeadEnd,
                DungeonDifficulty = dungeonOption.DungeonDifficulty,
                DungeonSize = dungeonOption.DungeonSize,
                MonsterType = dungeonOption.MonsterType,
                PartyLevel = dungeonOption.PartyLevel,
                PartySize = dungeonOption.PartySize,
                TrapPercent = dungeonOption.TrapPercent,
                RoamingPercent = dungeonOption.RoamingPercent,
                TreasureValue = dungeonOption.TreasureValue,
                RoomDensity = dungeonOption.RoomDensity,
                RoomSize = dungeonOption.RoomSize,
                Corridor = dungeonOption.Corridor,
                Id = dungeonOption.Id,
                UserId = dungeonOption.UserId
            };

            sd = await _dungeonService.GenerateDungeonAsync(model);
            sd.Level = 1;
            await _dungeonService.AddDungeonAsync(sd, token);
        }

        private async Task SeedUsers(CancellationToken token)
        {
            _context.Add(new User
            {
                Username = "TestAdmin",
                Password = PasswordHelper.EncryptPassword("adminPassword123"),
                FirstName = "Test",
                LastName = "Admin",
                Email = "admin@admin.com",
                Role = Role.Admin         
            });

            _context.Add(new User
            {
                Username = "TestUser",
                Password = PasswordHelper.EncryptPassword("simplePassword123"),
                FirstName = "Test",
                LastName = "User",
                Email = "user@user.com",
                Role = Role.User
            });
            await _context.SaveChangesAsync(token);
        }
    }
}