using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RDMG.Core.Abstractions.Dungeon;
using RDMG.Core.Abstractions.Repository;
using RDMG.Core.Abstractions.Services;
using RDMG.Core.Generator;
using RDMG.Core.Services;
using RDMG.Infrastructure.Data;
using RDMG.Infrastructure.Repository;
using RDMG.Infrastructure.Seed;
using System;
using System.Reflection;

namespace RDMG;

public static class ConfigureServices
{
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        switch (configuration.GetConnectionString("DbProvider").ToLower())
        {
            case Context.SqlServerContext:
                services.AddDbContext<Context>(options =>
                {
                    options.UseSqlServer(configuration.GetConnectionString("RDMG"));
                });
                services.AddDbContext<SqlServerContext>(options =>
                {
                    options.UseSqlServer(configuration.GetConnectionString("RDMG"),
                        sqlServerOptionsAction: sqlOptions =>
                        {
                            sqlOptions.MigrationsAssembly(typeof(SqlServerContext).GetTypeInfo().Assembly.GetName().Name);
                        });
                });
                break;
            case Context.SqliteContext:
                services.AddDbContext<Context>(options =>
                {
                    options.UseSqlite(configuration.GetConnectionString("RDMG"));
                });
                services.AddDbContext<SqliteContext>(options =>
                {
                    options.UseSqlite(configuration.GetConnectionString("RDMG"),
                        sqliteOptionsAction: sqlOptions =>
                        {
                            sqlOptions.MigrationsAssembly(typeof(SqliteContext).GetTypeInfo().Assembly.GetName().Name);
                        });
                });
                break;
            default:
                throw new Exception($"DbProvider not recognized: {configuration.GetConnectionString("DbProvider")}");

        }
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
            .AddScoped<IDungeonService, DungeonService>();

        return services;
    }

    public static IServiceCollection AddCookiePolicy(this IServiceCollection services)
    {
        services.Configure<CookiePolicyOptions>(options =>
        {
            // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            options.CheckConsentNeeded = _ => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });

        return services;
    }
}