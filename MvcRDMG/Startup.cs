using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MvcRDMG.Core.Abstractions.Repository;
using MvcRDMG.Core.Abstractions.Services;
using MvcRDMG.Core.Services;
using MvcRDMG.Infrastructure;
using MvcRDMG.Seed;
using System;
using System.Reflection;

namespace MvcRDMG
{
    public class Startup
    {
        private static IConfiguration Configuration;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews()
                    .SetCompatibilityVersion(CompatibilityVersion.Latest)
                    .AddRazorRuntimeCompilation();
            services.AddCookiePolicy()
                    .AddDatabase(Configuration)
                    .AddLogging(builder =>
                    {
                        builder.AddConfiguration(Configuration.GetSection("Logging"))
                            .AddConsole()
                            .AddDebug();
                    })
                    .AddHttpContextAccessor()
                    .AddApplicationServices()
                    .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
                    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.LoginPath = new PathString("/Auth/Login");
                        options.AccessDeniedPath = new PathString("/Auth/Forbidden/");
                    });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ContextSeedData seedData)
        {
            app.ConfigureDB(Configuration);
            if (env.EnvironmentName.StartsWith("Development"))
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            seedData.SeedDataAsync().Wait();
        }
    }

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder ConfigureDB(this IApplicationBuilder app, IConfiguration configuration)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

            switch (configuration.GetConnectionString("DbProvider").ToLower())
            {
                case "sqlserver":
                    MigrateDB<SqlServerContext>(serviceScope);
                    break;
                case "sqlite":
                    MigrateDB<SqliteContext>(serviceScope);
                    break;
                default:
                    throw new Exception($"DbProvider not recognized: {configuration.GetConnectionString("DbProvider")}");
            }

            return app;
        }
        public static void MigrateDB<T>(IServiceScope serviceScope) where T : Context
        {
            using var context = serviceScope.ServiceProvider.GetService<T>();
            context.Database.Migrate();
        }
    }

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            switch (configuration.GetConnectionString("DbProvider").ToLower())
            {
                case "sqlserver":
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
                case "sqlite":
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
                    .AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IUserService, UserService>()
                    .AddScoped<IAuthService, AuthService>()
                    .AddScoped<IDungeonService, DungeonService>()
                    .AddScoped<IDungeonGenerator, DungeonGenerator>();

            return services;
        }

        public static IServiceCollection AddCookiePolicy(this IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            return services;
        }
    }
}
