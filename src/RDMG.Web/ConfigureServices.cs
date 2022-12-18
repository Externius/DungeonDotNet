using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RDMG.Core.Abstractions.Data;
using RDMG.Core.Abstractions.Generator;
using RDMG.Core.Abstractions.Repository;
using RDMG.Core.Abstractions.Services;
using RDMG.Core.Generator;
using RDMG.Core.Services;
using RDMG.Infrastructure.Data;
using RDMG.Infrastructure.Repository;
using RDMG.Infrastructure.Seed;
using Serilog;
using System;
using System.IO;
using System.Reflection;

namespace RDMG.Web;

public static class ConfigureServices
{
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        switch (configuration.GetConnectionString(Context.DbProvider)?.ToLower())
        {
            case Context.SqlServerContext:
                services.AddDbContext<SqlServerContext>(options =>
                {
                    options.UseSqlServer(configuration.GetConnectionString(Context.Rdmg),
                        sqlServerOptionsAction: sqlOptions =>
                        {
                            sqlOptions.MigrationsAssembly(typeof(SqlServerContext).GetTypeInfo().Assembly.GetName().Name);
                        });
                });
                break;
            case Context.SqliteContext:
                services.AddDbContext<SqliteContext>(options =>
                {
                    options.UseSqlite(configuration.GetConnectionString(Context.Rdmg),
                        sqliteOptionsAction: sqlOptions =>
                        {
                            sqlOptions.MigrationsAssembly(typeof(SqliteContext).GetTypeInfo().Assembly.GetName().Name);
                        });
                });
                break;
            default:
                throw new Exception(
                    string.Format(Resources.Error.DbProviderError, configuration.GetConnectionString(Context.DbProvider)));
        }

        return services;
    }

    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
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

        switch (configuration.GetConnectionString(Context.DbProvider)?.ToLower())
        {
            case Context.SqlServerContext:
                services.AddScoped<IAppDbContext, SqlServerContext>(sp => sp.GetRequiredService<SqlServerContext>());
                break;
            case Context.SqliteContext:
                services.AddScoped<IAppDbContext, SqliteContext>(sp => sp.GetRequiredService<SqliteContext>());
                break;
            default:
                throw new Exception(
                    string.Format(Resources.Error.DbProviderError, configuration.GetConnectionString(Context.DbProvider)));
        }

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

    public static IHostBuilder AddSerilog(this IHostBuilder host,
        IConfiguration configuration)
    {
        switch (configuration.GetConnectionString(Context.DbProvider)?.ToLower())
        {
            case Context.SqlServerContext:
                host.UseSerilog((ctx, lc) => lc
                    .ReadFrom.Configuration(ctx.Configuration));
                break;
            case Context.SqliteContext:
                host.UseSerilog(new LoggerConfiguration()
                    .WriteTo.File($"Logs{Path.DirectorySeparatorChar}log.txt", rollingInterval: RollingInterval.Day)
                    .CreateLogger());
                break;
            default:
                throw new Exception(
                    string.Format(Resources.Error.DbProviderError,
                    configuration.GetConnectionString(Context.DbProvider)));
        }

        return host;
    }
}