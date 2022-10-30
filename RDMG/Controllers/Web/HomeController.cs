using Microsoft.AspNetCore.Mvc;
using RDMG.Models.Home;
using System.Diagnostics;

namespace RDMG.Controllers.Web;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        if (User.Identity is { IsAuthenticated: false })
        {
            return RedirectToAction("Login", "Auth");
        }
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}