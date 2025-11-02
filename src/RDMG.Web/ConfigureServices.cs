using System.Globalization;
using Mapster;
using Microsoft.AspNetCore.Authentication.Cookies;
using RDMG.Core.Abstractions.Services;
using RDMG.Core.Abstractions.Services.Exceptions;
using RDMG.Core.Abstractions.Services.Models;
using RDMG.Infrastructure.Data;
using RDMG.Web.Models.Dungeon;
using RDMG.Web.Services;
using Serilog;

namespace RDMG.Web;

public static class ConfigureServices
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor()
            .AddScoped<ICurrentUserService, CurrentUserService>();
        services.Configure<CookiePolicyOptions>(options =>
        {
            // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            options.CheckConsentNeeded = _ => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = new PathString("/Auth/Login");
                options.AccessDeniedPath = new PathString("/Auth/Forbidden/");
            });
        services.ConfigureMapster()
            .AddMemoryCache();

        services.AddMvc()
#if DEBUG
            .AddRazorRuntimeCompilation()
#endif
            ;

        services.AddWebOptimizer(pipeline =>
        {
            pipeline.AddJavaScriptBundle("js/site.min.js", "js/site.js", "js/password.js");
            pipeline.AddCssBundle("css/site.min.css", "css/site.css");
        });
        services.AddHealthChecks();

        return services;
    }

    private static IServiceCollection ConfigureMapster(this IServiceCollection services)
    {
        services.AddMapster();
        TypeAdapterConfig<DungeonOptionModel, DungeonOptionCreateViewModel>
            .NewConfig()
            .Ignore(dest => dest.DeadEnds)
            .Ignore(dest => dest.Corridors)
            .Ignore(dest => dest.DungeonSizes)
            .Ignore(dest => dest.DungeonDifficulties)
            .Ignore(dest => dest.PartyLevels)
            .Ignore(dest => dest.PartySizes)
            .Ignore(dest => dest.TreasureValues)
            .Ignore(dest => dest.ItemsRarities)
            .Ignore(dest => dest.RoomDensities)
            .Ignore(dest => dest.RoomSizes)
            .Ignore(dest => dest.MonsterTypes)
            .Ignore(dest => dest.TrapPercents)
            .Ignore(dest => dest.Themes)
            .Ignore(dest => dest.RoamingPercents)
            .Map(dest => dest.MonsterType, src => GetMonsters(src))
            .Map(dest => dest.TreasureValue, src => src.TreasureValue.ToString(CultureInfo.InvariantCulture));
        TypeAdapterConfig<DungeonOptionCreateViewModel, DungeonOptionModel>
            .NewConfig()
            .Ignore(dest => dest.MonsterType)
            .Map(dest => dest.TreasureValue, src => Convert.ToDouble(src.TreasureValue, CultureInfo.InvariantCulture));

        return services;
    }

    private static string[] GetMonsters(DungeonOptionModel model)
    {
        return model.MonsterType.Split(',');
    }

    public static IHostBuilder AddSerilog(this IHostBuilder host,
        IConfiguration configuration)
    {
        switch (configuration.GetConnectionString(AppDbContext.DbProvider)?.ToLower())
        {
            case AppDbContext.SqlServerContext:
                host.UseSerilog((ctx, lc) => lc
                    .ReadFrom.Configuration(ctx.Configuration));
                break;
            case AppDbContext.SqliteContext:
                host.UseSerilog(new LoggerConfiguration()
                    .WriteTo.File($"Logs{Path.DirectorySeparatorChar}log.txt", rollingInterval: RollingInterval.Day)
                    .CreateLogger());
                break;
            default:
                throw new ServiceException(
                    string.Format(Resources.Error.DbProviderError,
                        configuration.GetConnectionString(AppDbContext.DbProvider)));
        }

        return host;
    }
}