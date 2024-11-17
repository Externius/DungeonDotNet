using Microsoft.AspNetCore.Authentication.Cookies;
using RDMG.Core.Abstractions.Services;
using RDMG.Core.Abstractions.Services.Exceptions;
using RDMG.Infrastructure.Data;
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

        services.AddMemoryCache();

        services.AddMvc()
#if DEBUG
            .AddRazorRuntimeCompilation()
#endif
            ;

        services.AddHealthChecks();

        return services;
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