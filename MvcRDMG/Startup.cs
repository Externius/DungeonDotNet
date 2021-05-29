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
            UpdateDB(app);
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

        private static void UpdateDB(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<Context>();
            context.Database.Migrate();
        }
    }
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<Context>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString("RDMG"));
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
