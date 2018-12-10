using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MvcRDMG.Services;
using Microsoft.EntityFrameworkCore;
using MvcRDMG.Models;
using Microsoft.Extensions.Logging;
using AutoMapper;
using MvcRDMG.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using System.Net;
using Microsoft.AspNetCore.Authentication;

namespace MvcRDMG
{
    public class Startup
    {
        public static IConfigurationRoot Configuration;
        public Startup(IHostingEnvironment env)
        {
            HostingEnvironment = env;
            var builder = new ConfigurationBuilder()
             .SetBasePath(env.ContentRootPath)
             .AddJsonFile("config.json")
             .AddEnvironmentVariables();
            Configuration = builder.Build();

        }
        public IHostingEnvironment HostingEnvironment { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

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
            services.AddDbContext<UserContext>();
            services.AddIdentity<DungeonUser, IdentityRole>()
                .AddEntityFrameworkStores<UserContext>()
                .AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 6;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;
                options.User.RequireUniqueEmail = true;
            });
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.Expiration = TimeSpan.FromDays(150);
                options.LoginPath = "/Auth/Login";
                options.LogoutPath = "/Auth/Logout";
                options.AccessDeniedPath = "/Auth/AccessDenied";
                options.SlidingExpiration = true;
                options.Events.OnRedirectToAccessDenied = ReplaceRedirector(HttpStatusCode.Forbidden, options.Events.OnRedirectToAccessDenied);
                options.Events.OnRedirectToLogin = ReplaceRedirector(HttpStatusCode.Unauthorized, options.Events.OnRedirectToLogin);
            });
            services.AddEntityFrameworkSqlite()
                .AddDbContext<DungeonContext>();
            services.AddTransient<DungeonContextSeedData>();
            services.AddTransient<UserContextSeedData>();
            services.AddScoped<IDungeonRepository, DungeonRepository>();
            if (HostingEnvironment.IsDevelopment())
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
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env, DungeonContextSeedData seedData, UserContextSeedData userSeed)
        {
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            Mapper.Initialize(config =>
            {
                config.CreateMap<Option, OptionViewModel>().ReverseMap();
                config.CreateMap<SavedDungeon, SavedDungeonViewModel>().ReverseMap();
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            seedData.SeedData();
            await userSeed.SeedDataAsync();
        }
        static Func<RedirectContext<CookieAuthenticationOptions>, Task> ReplaceRedirector(HttpStatusCode statusCode, Func<RedirectContext<CookieAuthenticationOptions>, Task> existingRedirector) =>
        context =>
        {
            if (context.Request.Path.StartsWithSegments("/api"))
            {
                context.Response.StatusCode = (int)statusCode;
                return Task.CompletedTask;
            }
            return existingRedirector(context);
        };
    }
}
