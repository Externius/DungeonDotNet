using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RDMG.Core.Abstractions.Dungeon;
using RDMG.Core.Abstractions.Repository;
using RDMG.Core.Abstractions.Services;
using RDMG.Core.Generator;
using RDMG.Core.Services;
using RDMG.Infrastructure;
using RDMG.Infrastructure.Repository;
using System;
using System.Reflection;
using RDMG.Infrastructure.Seed;

namespace RDMG
{
    public class Startup
    {
        private static IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddCookiePolicy()
                    .AddDatabase(_configuration)
                    .AddLogging(builder =>
                    {
                        builder.AddFile(_configuration.GetSection("Logging"));
                    })
                    .AddHttpContextAccessor()
                    .AddApplicationServices()
                    .AddAutoMapper(cfg =>
                    {
                        cfg.AllowNullCollections = true;
                    }
                    , AppDomain.CurrentDomain.GetAssemblies())
                    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.LoginPath = new PathString("/Auth/Login");
                        options.AccessDeniedPath = new PathString("/Auth/Forbidden/");
                    });
            services.AddMemoryCache();
            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                })
#if DEBUG
                .AddRazorRuntimeCompilation()
#endif
                ;
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            app.ConfigureDb(_configuration);
            if (env.IsDevelopment())
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
        }
    }

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder ConfigureDb(this IApplicationBuilder app, IConfiguration configuration)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

            switch (configuration.GetConnectionString("DbProvider").ToLower())
            {
                case "sqlserver":
                    MigrateAndSeedDb<SqlServerContext>(serviceScope);
                    break;
                case "sqlite":
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
}
