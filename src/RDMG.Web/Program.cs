using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RDMG.Core;
using RDMG.Infrastructure;
using RDMG.Infrastructure.Data;
using RDMG.Web;
using System;
using System.Globalization;
using System.Threading;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddWebServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddApplicationServices();

builder.Host.AddSerilog(builder.Configuration);

var app = builder.Build();
var cultureInfo = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    try
    {
        var initializer = scope.ServiceProvider
            .GetRequiredService<AppDbContextInitializer>();
        var source = new CancellationTokenSource();
        var token = source.Token;
        initializer.UpdateAsync(token).Wait();
        initializer.SeedDataAsync(token).Wait();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider
            .GetRequiredService<ILogger<Program>>();

        logger.LogError(ex,
            "An error occurred during database initialization.");

        throw;
    }
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
