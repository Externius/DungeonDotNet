using RDMG.Core.Abstractions.Services;
using RDMG.Core.Abstractions.Services.Models;
using RDMG.Core.Domain;
using RDMG.Core.Helpers;
using RDMG.Infrastructure;
using System;
using System.Collections.Generic;
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
                var token = source.Token;
                await SeedUsers(token);
                await SeedOptions(token);
                await SeedDungeons(token);
            }
        }

        public async Task SeedBaseAsync()
        {
            if (!_context.Options.Any())
            {
                var source = new CancellationTokenSource();
                await SeedOptions(source.Token);
            }
        }

        private async Task SeedOptions(CancellationToken token)
        {
            await SeedSizeAsync(token);
            await SeedDifficultyAsync(token);
            await SeedTreasureValueAsync(token);
            await SeedItemsRarityAsync(token);
            await SeedRoomDensityAsync(token);
            await SeedRoomSizeAsync(token);
            await SeedMonsterTypeAsync(token);
            await SeedTrapPercentAsync(token);
            await SeedRoamingPercentAsync(token);
            await SeedThemeAsync(token);
        }

        private async Task SeedThemeAsync(CancellationToken token)
        {
            var options = new List<Option>
            {
                new Option()
                {
                    Key = OptionKey.Theme,
                    Name = Resources.Dungeon.Dark,
                    Value = "0"
                },
                new Option()
                {
                    Key = OptionKey.Theme,
                    Name = Resources.Dungeon.Light,
                    Value = "1"
                },
                new Option()
                {
                    Key = OptionKey.Theme,
                    Name = Resources.Dungeon.Minimal,
                    Value = "2"
                }
            };
            await _context.Options.AddRangeAsync(options, token);
            await _context.SaveChangesAsync(token);
        }

        private async Task SeedRoamingPercentAsync(CancellationToken token)
        {
            var options = new List<Option>
            {
                new Option()
                {
                    Key = OptionKey.RoamingPercent,
                    Name = "None",
                    Value = "0"
                },
                new Option()
                {
                    Key = OptionKey.RoamingPercent,
                    Name = "Few",
                    Value = "10"
                },
                new Option()
                {
                    Key = OptionKey.RoamingPercent,
                    Name = "More",
                    Value = "20"
                }
            };
            await _context.Options.AddRangeAsync(options, token);
            await _context.SaveChangesAsync(token);
        }

        private async Task SeedTrapPercentAsync(CancellationToken token)
        {
            var options = new List<Option>
            {
                new Option()
                {
                    Key = OptionKey.TrapPercent,
                    Name = "None",
                    Value = "0"
                },
                new Option()
                {
                    Key = OptionKey.TrapPercent,
                    Name = "Few",
                    Value = "15"
                },
                new Option()
                {
                    Key = OptionKey.TrapPercent,
                    Name = "More",
                    Value = "30"
                }
            };
            await _context.Options.AddRangeAsync(options, token);
            await _context.SaveChangesAsync(token);
        }

        private async Task SeedMonsterTypeAsync(CancellationToken token)
        {
            var options = new List<Option>
            {
                new Option()
                {
                    Key = OptionKey.MonsterType,
                    Name = "Aberrations",
                    Value = "aberration"
                },
                new Option()
                {
                    Key = OptionKey.MonsterType,
                    Name = "Beasts",
                    Value = "beast"
                },
                new Option()
                {
                    Key = OptionKey.MonsterType,
                    Name = "Celestials",
                    Value = "celestial"
                },
                new Option()
                {
                    Key = OptionKey.MonsterType,
                    Name = "Constructs",
                    Value = "construct"
                },
                new Option()
                {
                    Key = OptionKey.MonsterType,
                    Name = "Dragons",
                    Value = "dragon"
                },
                new Option()
                {
                    Key = OptionKey.MonsterType,
                    Name = "Elementals",
                    Value = "elemental"
                },
                new Option()
                {
                    Key = OptionKey.MonsterType,
                    Name = "Fey",
                    Value = "fey"
                },
                new Option()
                {
                    Key = OptionKey.MonsterType,
                    Name = "Fiends",
                    Value = "fiend"
                },
                new Option()
                {
                    Key = OptionKey.MonsterType,
                    Name = "Giants",
                    Value = "giant"
                },
                new Option()
                {
                    Key = OptionKey.MonsterType,
                    Name = "Humanoids",
                    Value = "humanoid"
                },
                new Option()
                {
                    Key = OptionKey.MonsterType,
                    Name = "Monstrosities",
                    Value = "monstrosity"
                },
                new Option()
                {
                    Key = OptionKey.MonsterType,
                    Name = "Oozes",
                    Value = "ooze"
                },
                new Option()
                {
                    Key = OptionKey.MonsterType,
                    Name = "Plants",
                    Value = "plant"
                },
                new Option()
                {
                    Key = OptionKey.MonsterType,
                    Name = "Swarm of tiny beasts",
                    Value = "swarm of Tiny beasts"
                },
                new Option()
                {
                    Key = OptionKey.MonsterType,
                    Name = "Undead",
                    Value = "undead"
                }
            };
            await _context.Options.AddRangeAsync(options, token);
            await _context.SaveChangesAsync(token);
        }

        private async Task SeedRoomSizeAsync(CancellationToken token)
        {
            var options = new List<Option>
            {
                new Option()
                {
                    Key = OptionKey.RoomSize,
                    Name = "Small",
                    Value = "20"
                },
                new Option()
                {
                    Key = OptionKey.RoomSize,
                    Name = "Medium",
                    Value = "35"
                },
                new Option()
                {
                    Key = OptionKey.RoomSize,
                    Name = "Large",
                    Value = "45"
                }
            };
            await _context.Options.AddRangeAsync(options, token);
            await _context.SaveChangesAsync(token);
        }

        private async Task SeedRoomDensityAsync(CancellationToken token)
        {
            var options = new List<Option>
            {
                new Option()
                {
                    Key = OptionKey.RoomDensity,
                    Name = "Low",
                    Value = "20"
                },
                new Option()
                {
                    Key = OptionKey.RoomDensity,
                    Name = "Medium",
                    Value = "30"
                },
                new Option()
                {
                    Key = OptionKey.RoomDensity,
                    Name = "High",
                    Value = "40"
                }
            };
            await _context.Options.AddRangeAsync(options, token);
            await _context.SaveChangesAsync(token);
        }

        private async Task SeedItemsRarityAsync(CancellationToken token)
        {
            var options = new List<Option>
            {
                new Option()
                {
                    Key = OptionKey.ItemsRarity,
                    Name = "Common",
                    Value = "0"
                },
                new Option()
                {
                    Key = OptionKey.ItemsRarity,
                    Name = "Unommon",
                    Value = "1"
                },
                new Option()
                {
                    Key = OptionKey.ItemsRarity,
                    Name = "Rare",
                    Value = "2"
                },
                new Option()
                {
                    Key = OptionKey.ItemsRarity,
                    Name = "Very Rare",
                    Value = "3"
                },
                new Option()
                {
                    Key = OptionKey.ItemsRarity,
                    Name = "Legendary",
                    Value = "4"
                }
            };
            await _context.Options.AddRangeAsync(options, token);
            await _context.SaveChangesAsync(token);
        }

        private async Task SeedTreasureValueAsync(CancellationToken token)
        {
            var options = new List<Option>
            {
                new Option()
                {
                    Key = OptionKey.TreasureValue,
                    Name = "Low",
                    Value = "0.5"
                },
                new Option()
                {
                    Key = OptionKey.TreasureValue,
                    Name = "Standard",
                    Value = "1"
                },
                new Option()
                {
                    Key = OptionKey.TreasureValue,
                    Name = "High",
                    Value = "1.5"
                }
            };
            await _context.Options.AddRangeAsync(options, token);
            await _context.SaveChangesAsync(token);
        }

        private async Task SeedDifficultyAsync(CancellationToken token)
        {
            var options = new List<Option>
            {
                new Option()
                {
                    Key = OptionKey.Difficulty,
                    Name = "Easy",
                    Value = "0"
                },
                new Option()
                {
                    Key = OptionKey.Difficulty,
                    Name = "Medium",
                    Value = "1"
                },
                new Option()
                {
                    Key = OptionKey.Difficulty,
                    Name = "Hard",
                    Value = "2"
                },
                new Option()
                {
                    Key = OptionKey.Difficulty,
                    Name = "Deadly",
                    Value = "3"
                }
            };
            await _context.Options.AddRangeAsync(options, token);
            await _context.SaveChangesAsync(token);
        }

        private async Task SeedSizeAsync(CancellationToken token)
        {
            var options = new List<Option>
            {
                new Option()
                {
                    Key = OptionKey.Size,
                    Name = "Small",
                    Value = "20"
                },
                new Option()
                {
                    Key = OptionKey.Size,
                    Name = "Medium",
                    Value = "32"
                },
                new Option()
                {
                    Key = OptionKey.Size,
                    Name = "Large",
                    Value = "44"
                }
            };
            await _context.Options.AddRangeAsync(options, token);
            await _context.SaveChangesAsync(token);
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