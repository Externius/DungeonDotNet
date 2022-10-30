using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RDMG.Core.Abstractions.Services;
using RDMG.Infrastructure.Data;
using RDMG.Infrastructure.Seed;
using System;

namespace RDMG.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder ConfigureDb(this IApplicationBuilder app, IConfiguration configuration)
    {
        using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

        switch (configuration.GetConnectionString("DbProvider").ToLower())
        {
            case Context.SqlServerContext:
                MigrateAndSeedDb<SqlServerContext>(serviceScope);
                break;
            case Context.SqliteContext:
                MigrateAndSeedDb<SqliteContext>(serviceScope);
                break;
            default:
                throw new Exception($"DbProvider not recognized: {configuration.GetConnectionString("DbProvider")}");
        }

        return app;
    }

    public static void MigrateAndSeedDb<T>(IServiceScope serviceScope) where T : Context
    {
        using var context = serviceScope.ServiceProvider.GetService<T>();
        context?.Database.Migrate();
        var service = serviceScope.ServiceProvider.GetService<IDungeonService>();
        var seedData = new ContextSeedData(service, context);
        seedData.SeedDataAsync().Wait();
    }
}