using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RDMG.Core.Abstractions.Dungeon;
using RDMG.Core.Abstractions.Dungeon.Models;
using RDMG.Core.Abstractions.Repository;
using RDMG.Core.Abstractions.Services;
using RDMG.Core.Abstractions.Services.Models;
using RDMG.Core.Generator;
using RDMG.Core.Services;
using RDMG.Infrastructure;
using RDMG.Seed;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace RDMG.Tests
{
    public class TestEnvironment : IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IServiceScope _scope;
        private SqliteConnection Connection { get; }

        public TestEnvironment()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            var configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
            configurationBuilder.AddJsonFile(configFile);

            Connection = new SqliteConnection("DataSource=:memory:");
            Connection.Open();

            var services = new ServiceCollection();
            ConfigureServices(services);

            _serviceProvider = services.BuildServiceProvider();
            _scope = _serviceProvider.CreateScope();

            MigrateDbContext();
            SeedDataAsync().Wait();
        }

        private async Task SeedDataAsync()
        {
            var context = _scope.ServiceProvider.GetService<SqliteContext>();
            var service = _scope.ServiceProvider.GetService<IDungeonService>();
            var seedData = new ContextSeedData(service, context);
            await seedData.SeedDataAsync();
        }

        public T GetService<T>()
        {
            return _scope.ServiceProvider.GetService<T>();
        }

        public Dungeon GetDungeon(OptionModel optionModel = null)
        {
            var helperService = _scope.ServiceProvider.GetService<IDungeonHelper>();

            if (optionModel is null)
            {
                optionModel = new OptionModel
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
            }

            helperService.Init(optionModel);

            var model = new DungeonOptionsModel
            {
                Size = optionModel.DungeonSize,
                RoomDensity = optionModel.RoomDensity,
                RoomSizePercent = optionModel.RoomSize,
                TrapPercent = optionModel.TrapPercent,
                HasDeadEnds = optionModel.DeadEnd,
                RoamingPercent = optionModel.RoamingPercent
            };

            return new Dungeon(model, helperService);
        }

        public DungeonNoCorridor GetNCDungeon(OptionModel optionModel = null)
        {
            var helperService = _scope.ServiceProvider.GetService<IDungeonHelper>();

            if (optionModel is null)
            {
                optionModel = new OptionModel
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
            }

            helperService.Init(optionModel);

            return new DungeonNoCorridor(800, 800, 15, 15, helperService);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions()
                .AddDatabase(Connection)
                .AddApplicationServices()
                .AddAutoMapper(new Type[] { typeof(Core.Services.Automapper.DungeonProfile)
                , typeof(Core.Services.Automapper.UserProfile)
                , typeof(Infrastructure.Repository.Automapper.DungeonProfile)
                , typeof(Infrastructure.Repository.Automapper.UserProfile)
                })
                .AddLogging();
        }

        private void MigrateDbContext()
        {
            using var scope = _serviceProvider.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetService<SqliteContext>();
            context.Database.Migrate();
        }

        public void Dispose()
        {
            _scope?.Dispose();
            Connection?.Dispose();
        }
    }

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services,
             SqliteConnection connection)
        {
            services.AddDbContext<Context>(options =>
            {
                options.UseSqlite(connection);
            });
            services.AddDbContext<SqliteContext>(options =>
            {
                options.UseSqlite(connection,
                sqliteOptionsAction: sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(SqliteContext).GetTypeInfo().Assembly.GetName().Name);
                });
                options.ConfigureWarnings(wcb => wcb.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.AmbientTransactionWarning));
            });

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<ContextSeedData>()
                    .AddScoped<IDungeonRepository, DungeonRepository>()
                    .AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IUserService, UserService>()
                    .AddScoped<IAuthService, AuthService>()
                    .AddScoped<IDungeonHelper, DungeonHelper>()
                    .AddScoped<IDungeonService, DungeonService>();

            return services;
        }
    }
}
