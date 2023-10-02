using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RDMG.Core.Abstractions.Data;
using RDMG.Core.Abstractions.Repository;
using RDMG.Core.Abstractions.Services.Exceptions;
using RDMG.Infrastructure.Data;
using RDMG.Infrastructure.Interceptors;
using RDMG.Infrastructure.Repository;
using System.Reflection;

namespace RDMG.Infrastructure;
public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        switch (configuration.GetConnectionString(AppDbContext.DbProvider)?.ToLower())
        {
            case AppDbContext.SqlServerContext:
                services.AddDbContext<SqlServerContext>((sp, options) =>
                {
                    options.UseSqlServer(configuration.GetConnectionString(AppDbContext.Rdmg),
                        sqlServerOptionsAction: sqlOptions =>
                        {
                            sqlOptions.MigrationsAssembly(typeof(SqlServerContext).GetTypeInfo().Assembly.GetName().Name);
                        })
                        .AddInterceptors(
                            ActivatorUtilities.CreateInstance<AuditEntitiesSaveChangesInterceptor>(sp));
                });

                services.AddScoped<IAppDbContext, SqlServerContext>(sp =>
                        sp.GetRequiredService<SqlServerContext>())
                    .AddScoped<AppDbContextInitializer>();
                break;
            case AppDbContext.SqliteContext:
                services.AddDbContext<SqliteContext>((sp, options) =>
                {
                    options.UseSqlite(configuration.GetConnectionString(AppDbContext.Rdmg),
                        sqliteOptionsAction: sqlOptions =>
                        {
                            sqlOptions.MigrationsAssembly(typeof(SqliteContext).GetTypeInfo().Assembly.GetName().Name);
                        })
                        .AddInterceptors(
                            ActivatorUtilities.CreateInstance<AuditEntitiesSaveChangesInterceptor>(sp));
                });

                services.AddScoped<IAppDbContext, SqliteContext>(sp =>
                        sp.GetRequiredService<SqliteContext>())
                    .AddScoped<AppDbContextInitializer>();
                break;
            default:
                throw new ServiceException(
                    string.Format(Resources.Error.DbProviderError, configuration.GetConnectionString(AppDbContext.DbProvider)));
        }

        services
            .AddScoped<IDungeonRepository, DungeonRepository>()
            .AddScoped<IDungeonOptionRepository, DungeonOptionRepository>()
            .AddScoped<IOptionRepository, OptionRepository>()
            .AddScoped<IUserRepository, UserRepository>();

        return services;
    }

    public static IServiceCollection AddTestInfrastructureServices(this IServiceCollection services,
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
        services.AddScoped<IAppDbContext, SqliteContext>(sp =>
                sp.GetRequiredService<SqliteContext>())
            .AddScoped<AppDbContextInitializer>();

        services
            .AddScoped<IDungeonRepository, DungeonRepository>()
            .AddScoped<IDungeonOptionRepository, DungeonOptionRepository>()
            .AddScoped<IOptionRepository, OptionRepository>()
            .AddScoped<IUserRepository, UserRepository>();
        return services;
    }
}