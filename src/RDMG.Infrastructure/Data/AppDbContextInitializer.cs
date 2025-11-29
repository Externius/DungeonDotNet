using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RDMG.Core.Abstractions.Configuration;
using RDMG.Core.Abstractions.Data;
using RDMG.Core.Abstractions.Services;
using RDMG.Core.Abstractions.Services.Models;
using RDMG.Core.Domain;
using RDMG.Core.Helpers;

namespace RDMG.Infrastructure.Data;

public class AppDbContextInitializer(IAppDbContext context, IDungeonService dungeonService, IOptions<AppConfig> config)
{
    private readonly IAppDbContext _context = context;
    private readonly IDungeonService _dungeonService = dungeonService;
    private const string UtDungeonName1 = "Test 1";
    public const string UtDungeonName2 = "Test 2";
    public const int TestAdminUserId = 1;
    public const int TestUserId = 2;
    public const int TestDeletedUserId = 3;
    public const int TestNotExistingUserId = 999;

    public async Task UpdateAsync(CancellationToken cancellationToken)
    {
        if (_context.Database.IsSqlServer() || _context.Database.IsSqlite())
        {
            await _context.Database.MigrateAsync(cancellationToken: cancellationToken);
        }
        else
        {
            await _context.Database.EnsureCreatedAsync(cancellationToken);
        }
    }

    public async Task SeedDataAsync(CancellationToken cancellationToken)
    {
        if (!_context.Users.Any())
        {
            await SeedUsersAsync(cancellationToken);
            await SeedOptionsAsync(cancellationToken);
            await SeedDungeonsAsync(TestAdminUserId, cancellationToken);
        }
    }

    public async Task SeedTestBaseAsync(CancellationToken cancellationToken)
    {
        if (!_context.Users.Any())
        {
            await SeedUsersAsync(cancellationToken, true);
            await SeedOptionsAsync(cancellationToken);
            await SeedDungeonsAsync(TestAdminUserId, cancellationToken);
            await SeedDungeonsAsync(TestUserId, cancellationToken);
        }
    }

    private async Task SeedOptionsAsync(CancellationToken token)
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
            new()
            {
                Key = OptionKey.Theme,
                Name = Resources.Dungeon.Dark,
                Value = "0"
            },
            new()
            {
                Key = OptionKey.Theme,
                Name = Resources.Dungeon.Light,
                Value = "1"
            },
            new()
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
            new()
            {
                Key = OptionKey.RoamingPercent,
                Name = Resources.Common.None,
                Value = "0"
            },
            new()
            {
                Key = OptionKey.RoamingPercent,
                Name = Resources.Common.Few,
                Value = "10"
            },
            new()
            {
                Key = OptionKey.RoamingPercent,
                Name = Resources.Common.More,
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
            new()
            {
                Key = OptionKey.TrapPercent,
                Name = Resources.Common.None,
                Value = "0"
            },
            new()
            {
                Key = OptionKey.TrapPercent,
                Name = Resources.Common.Few,
                Value = "15"
            },
            new()
            {
                Key = OptionKey.TrapPercent,
                Name = Resources.Common.More,
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
            new()
            {
                Key = OptionKey.MonsterType,
                Name = "Aberrations",
                Value = "aberration"
            },
            new()
            {
                Key = OptionKey.MonsterType,
                Name = "Beasts",
                Value = "beast"
            },
            new()
            {
                Key = OptionKey.MonsterType,
                Name = "Celestials",
                Value = "celestial"
            },
            new()
            {
                Key = OptionKey.MonsterType,
                Name = "Constructs",
                Value = "construct"
            },
            new()
            {
                Key = OptionKey.MonsterType,
                Name = "Dragons",
                Value = "dragon"
            },
            new()
            {
                Key = OptionKey.MonsterType,
                Name = "Elementals",
                Value = "elemental"
            },
            new()
            {
                Key = OptionKey.MonsterType,
                Name = "Fey",
                Value = "fey"
            },
            new()
            {
                Key = OptionKey.MonsterType,
                Name = "Fiends",
                Value = "fiend"
            },
            new()
            {
                Key = OptionKey.MonsterType,
                Name = "Giants",
                Value = "giant"
            },
            new()
            {
                Key = OptionKey.MonsterType,
                Name = "Humanoids",
                Value = "humanoid"
            },
            new()
            {
                Key = OptionKey.MonsterType,
                Name = "Monstrosities",
                Value = "monstrosity"
            },
            new()
            {
                Key = OptionKey.MonsterType,
                Name = "Oozes",
                Value = "ooze"
            },
            new()
            {
                Key = OptionKey.MonsterType,
                Name = "Plants",
                Value = "plant"
            },
            new()
            {
                Key = OptionKey.MonsterType,
                Name = "Swarm of tiny beasts",
                Value = "swarm of Tiny beasts"
            },
            new()
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
            new()
            {
                Key = OptionKey.RoomSize,
                Name = Resources.Common.Small,
                Value = "20"
            },
            new()
            {
                Key = OptionKey.RoomSize,
                Name = Resources.Common.Medium,
                Value = "35"
            },
            new()
            {
                Key = OptionKey.RoomSize,
                Name = Resources.Common.Large,
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
            new()
            {
                Key = OptionKey.RoomDensity,
                Name = Resources.Common.Low,
                Value = "20"
            },
            new()
            {
                Key = OptionKey.RoomDensity,
                Name = Resources.Common.Medium,
                Value = "30"
            },
            new()
            {
                Key = OptionKey.RoomDensity,
                Name = Resources.Common.High,
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
            new()
            {
                Key = OptionKey.ItemsRarity,
                Name = Resources.Common.CommonValue,
                Value = "0"
            },
            new()
            {
                Key = OptionKey.ItemsRarity,
                Name = Resources.Common.Uncommon,
                Value = "1"
            },
            new()
            {
                Key = OptionKey.ItemsRarity,
                Name = Resources.Common.Rare,
                Value = "2"
            },
            new()
            {
                Key = OptionKey.ItemsRarity,
                Name = Resources.Common.VeryRare,
                Value = "3"
            },
            new()
            {
                Key = OptionKey.ItemsRarity,
                Name = Resources.Common.Legendary,
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
            new()
            {
                Key = OptionKey.TreasureValue,
                Name = Resources.Common.Low,
                Value = "0.5"
            },
            new()
            {
                Key = OptionKey.TreasureValue,
                Name = Resources.Common.Standard,
                Value = "1"
            },
            new()
            {
                Key = OptionKey.TreasureValue,
                Name = Resources.Common.High,
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
            new()
            {
                Key = OptionKey.Difficulty,
                Name = Resources.Common.Easy,
                Value = "0"
            },
            new()
            {
                Key = OptionKey.Difficulty,
                Name = Resources.Common.Medium,
                Value = "1"
            },
            new()
            {
                Key = OptionKey.Difficulty,
                Name = Resources.Common.Hard,
                Value = "2"
            },
            new()
            {
                Key = OptionKey.Difficulty,
                Name = Resources.Common.Deadly,
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
            new()
            {
                Key = OptionKey.Size,
                Name = Resources.Common.Small,
                Value = "20"
            },
            new()
            {
                Key = OptionKey.Size,
                Name = Resources.Common.Medium,
                Value = "32"
            },
            new()
            {
                Key = OptionKey.Size,
                Name = Resources.Common.Large,
                Value = "44"
            }
        };
        await _context.Options.AddRangeAsync(options, token);
        await _context.SaveChangesAsync(token);
    }

    private async Task SeedDungeonsAsync(int userId, CancellationToken token)
    {
        var dungeonOption = new DungeonOption
        {
            UserId = userId,
            DungeonName = UtDungeonName1,
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

        var model = new DungeonOptionModel
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

        var sd = await _dungeonService.GenerateDungeonAsync(model, token);
        sd.Level = 1;
        await _dungeonService.AddDungeonAsync(sd, token);

        dungeonOption = new DungeonOption
        {
            UserId = userId,
            DungeonName = UtDungeonName2,
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

        model = new DungeonOptionModel
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

        sd = await _dungeonService.GenerateDungeonAsync(model, token);
        sd.Level = 1;
        await _dungeonService.AddDungeonAsync(sd, token);
    }

    private async Task SeedUsersAsync(CancellationToken token, bool seedDeletedUtUser = false)
    {
        _context.Users.Add(new User
        {
            Id = TestAdminUserId,
            Username = "TestAdmin",
            Password = PasswordHelper.EncryptPassword(config.Value.DefaultAdminPassword),
            FirstName = "Test",
            LastName = "Admin",
            Email = "admin@admin.com",
            Role = Role.Admin
        });

        _context.Users.Add(new User
        {
            Id = TestUserId,
            Username = "TestUser",
            Password = PasswordHelper.EncryptPassword(config.Value.DefaultUserPassword),
            FirstName = "Test",
            LastName = "User",
            Email = "user@user.com",
            Role = Role.User
        });

        if (seedDeletedUtUser)
        {
            _context.Users.Add(new User
            {
                Id = TestDeletedUserId,
                Username = "UT Deleted User",
                Password = PasswordHelper.EncryptPassword(config.Value.DefaultUserPassword),
                FirstName = "Test",
                LastName = "User",
                Email = "user@user.com",
                Role = Role.User,
                IsDeleted = true
            });
        }

        await _context.SaveChangesAsync(token);
    }
}