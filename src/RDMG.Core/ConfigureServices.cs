using Microsoft.Extensions.DependencyInjection;
using RDMG.Core.Abstractions.Generator;
using RDMG.Core.Abstractions.Services;
using RDMG.Core.Generator;
using RDMG.Core.Services;
using System;

namespace RDMG.Core;
public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services
    )
    {
        services
            .AddScoped<IUserService, UserService>()
            .AddScoped<IAuthService, AuthService>()
            .AddScoped<IDungeonHelper, DungeonHelper>()
            .AddScoped<IOptionService, OptionService>()
            .AddScoped<IDungeon, Dungeon>()
            .AddScoped<IDungeonNoCorridor, DungeonNoCorridor>()
            .AddScoped<IDungeonService, DungeonService>();

        services.AddAutoMapper(cfg =>
            {
                cfg.AllowNullCollections = true;
            }
            , AppDomain.CurrentDomain.GetAssemblies());

        return services;
    }
}