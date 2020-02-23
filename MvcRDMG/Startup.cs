using AutoMapper;
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
        public static IConfigurationRoot Configuration;
        public Startup(IWebHostEnvironment env)
        {
            HostingEnvironment = env;
            var builder = new ConfigurationBuilder()
             .SetBasePath(env.ContentRootPath)
             .AddJsonFile("config.json")
             .AddEnvironmentVariables();
            Configuration = builder.Build();
        }
        public IWebHostEnvironment HostingEnvironment { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddRazorPages();  
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddLogging(builder =>
            {
                builder.AddConfiguration(Configuration.GetSection("Logging"))
                    .AddConsole()
                    .AddDebug();
            });
            
            services.AddEntityFrameworkSqlite()
                        .AddDbContext<Context>(options => options.UseSqlite(Startup.Configuration["Data:DungeonContextConnection"]));

            AddApplicationServices(services);
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.LoginPath = "/Auth/Login";
                    });

            services.AddHttpContextAccessor();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        private void AddApplicationServices(IServiceCollection services)
        {
            services.AddTransient<ContextSeedData>();
            services.AddScoped<IDungeonRepository, DungeonRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDungeonService, DungeonService>();

            if (HostingEnvironment.EnvironmentName.StartsWith("Development"))
            {
                services.AddScoped<IMailService, DebugMailService>();
                services.AddScoped<IDungeonGenerator, DungeonGenerator>();
            }
            else
            {
                services.AddScoped<IMailService, DebugMailService>();
                services.AddScoped<IDungeonGenerator, DungeonGenerator>();
            }
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

        private void UpdateDB(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<Context>();
            context.Database.Migrate();
        }
    }
}
