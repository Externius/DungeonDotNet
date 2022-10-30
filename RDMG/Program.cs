using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RDMG.Extensions;
using RDMG.Infrastructure.Data;
using Serilog;
using System;
using System.Globalization;
using System.IO;

namespace RDMG;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllersWithViews();
        builder.Services.AddCookiePolicy()
            .AddDatabase(builder.Configuration)
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
        builder.Services.AddMemoryCache();
        builder.Services.AddMvc()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            })
#if DEBUG
            .AddRazorRuntimeCompilation()
#endif
            ;

        builder.Services.AddHealthChecks();
        AddSerilog(builder);

        var app = builder.Build();
        var cultureInfo = new CultureInfo("en-US");
        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

        app.ConfigureDb(builder.Configuration);
        if (app.Environment.IsDevelopment())
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
            endpoints.MapHealthChecks("/health");
        });

        app.Run();
    }

    private static void AddSerilog(WebApplicationBuilder builder)
    {
        switch (builder.Configuration.GetConnectionString("DbProvider").ToLower())
        {
            case Context.SqlServerContext:
                builder.Host.UseSerilog((ctx, lc) => lc
                    .ReadFrom.Configuration(ctx.Configuration));
                break;
            case Context.SqliteContext:
                builder.Host.UseSerilog(new LoggerConfiguration()
                    .WriteTo.File($"Logs{Path.DirectorySeparatorChar}log.txt", rollingInterval: RollingInterval.Day)
                    .CreateLogger());
                break;
            default:
                throw new Exception(
                    $"DbProvider not recognized: {builder.Configuration.GetConnectionString("DbProvider")}");
        }
    }
}