using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RDMG.Core.Abstractions.Data;
using RDMG.Core.Abstractions.Generator;
using RDMG.Core.Abstractions.Repository;
using RDMG.Core.Abstractions.Services;
using RDMG.Core.Abstractions.Services.Models;
using RDMG.Core.Generator;
using RDMG.Core.Services;
using RDMG.Infrastructure.Data;
using RDMG.Infrastructure.Repository;
using RDMG.Infrastructure.Seed;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace RDMG.Tests;

public class TestEnvironment : IDisposable
{
    private readonly IServiceScope _scope;
    private SqliteConnection Connection { get; }
    private bool _disposedValue;
    public TestEnvironment()
    {
        IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
        var configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
        configurationBuilder.AddJsonFile(configFile);

        Connection = new SqliteConnection("DataSource=:memory:");
        Connection.Open();

        var services = new ServiceCollection();
        ConfigureServices(services);
        MigrateAndSeedDbAsync(services).Wait();

        _scope = services.BuildServiceProvider().CreateScope();
    }

    public T GetService<T>()
    {
        return _scope.ServiceProvider.GetService<T>();
    }

    public IDungeon GetDungeon(DungeonOptionModel optionModel = null)
    {
        var service = _scope.ServiceProvider.GetService<IDungeon>();

        optionModel ??= new DungeonOptionModel
        {
            DungeonName = "UT Dungeon",
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

        service?.Init(optionModel);
        return service;
    }

    public IDungeonNoCorridor GetNcDungeon(DungeonOptionModel optionModel = null)
    {
        var service = _scope.ServiceProvider.GetService<IDungeonNoCorridor>();

        optionModel ??= new DungeonOptionModel
        {
            DungeonName = "UT Dungeon",
            Created = DateTime.UtcNow,
            ItemsRarity = 1,
            DeadEnd = true,
            DungeonDifficulty = 1,
            DungeonSize = 15,
            MonsterType = "any",
            PartyLevel = 4,
            PartySize = 4,
            TrapPercent = 20,
            RoamingPercent = 0,
            TreasureValue = 1,
            RoomDensity = 10,
            RoomSize = 15,
            Corridor = false
        };

        service?.Init(optionModel);
        return service;
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddOptions()
            .AddDatabase(Connection)
            .AddApplicationServices()
            .AddMemoryCache()
            .AddAutoMapper(cfg =>
                {
                    cfg.AllowNullCollections = true;
                }, typeof(Core.Services.Automapper.DungeonProfile),
                typeof(Core.Services.Automapper.UserProfile),
                typeof(Core.Services.Automapper.OptionProfile),
                typeof(Infrastructure.Repository.Automapper.DungeonProfile),
                typeof(Infrastructure.Repository.Automapper.UserProfile))
            .AddLogging();
    }

    private static async Task MigrateAndSeedDbAsync(IServiceCollection serviceCollection)
    {
        using var scope = serviceCollection.BuildServiceProvider().CreateScope();
        await using var context = scope.ServiceProvider.GetService<SqliteContext>();
        await context?.Database.MigrateAsync()!;
        var service = scope.ServiceProvider.GetService<IDungeonService>();
        var seedData = new ContextSeedData(service, context);
        await seedData.SeedDataAsync();
    }

    ~TestEnvironment() => Dispose(false);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposedValue)
            return;

        if (disposing)
        {
            Connection?.Dispose();
            _scope?.Dispose();
        }

        _disposedValue = true;
    }
}

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services,
        SqliteConnection connection)
    {
        services.AddDbContext<SqliteContext>(options =>
        {
            options.UseSqlite(connection,
                sqliteOptionsAction: sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(SqliteContext).GetTypeInfo().Assembly.GetName().Name);
                });
        });

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<ContextSeedData>()
            .AddScoped<IDungeonRepository, DungeonRepository>()
            .AddScoped<IDungeonOptionRepository, DungeonOptionRepository>()
            .AddScoped<IOptionRepository, OptionRepository>()
            .AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IUserService, UserService>()
            .AddScoped<IAuthService, AuthService>()
            .AddScoped<IDungeonHelper, DungeonHelper>()
            .AddScoped<IOptionService, OptionService>()
            .AddScoped<IDungeon, Dungeon>()
            .AddScoped<IDungeonNoCorridor, DungeonNoCorridor>()
            .AddScoped<IDungeonService, DungeonService>();

        services.AddScoped<IAppDbContext, SqliteContext>();

        return services;
    }
}