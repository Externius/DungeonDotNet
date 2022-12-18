using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RDMG.Web.Extensions;
using System;
using System.Globalization;

namespace RDMG.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllersWithViews();
        builder.Services.AddCookiePolicy()
            .AddDatabase(builder.Configuration)
            .AddHttpContextAccessor()
            .AddApplicationServices(builder.Configuration)
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
#if DEBUG
            .AddRazorRuntimeCompilation()
#endif
            ;

        builder.Services.AddHealthChecks();
        builder.Host.AddSerilog(builder.Configuration);

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

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapHealthChecks("/health");

        app.Run();
    }
}