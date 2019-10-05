using Microsoft.AspNetCore.Mvc;
using MvcRDMG.Core.Abstractions.Services;
using MvcRDMG.Models.Home;
using System.Diagnostics;

namespace MvcRDMG.Controllers.Web
{
    public class HomeController : Controller
    {
        private readonly IMailService _mailService;

        public HomeController(IMailService service)
        {
            _mailService = service;
        }

        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Auth");
            }
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                var email = Startup.Configuration["AppSettings:SiteEmailAddress"];

                if (string.IsNullOrWhiteSpace(email))
                {
                    ModelState.AddModelError("", "Could not send email, configuration problem.");
                }
                if (_mailService.SendMail(
                    email,
                    email,
                    $"Contact Page from {model.Name} ({model.Email})",
                    model.Message
                ))
                {
                    ModelState.Clear();

                    ViewBag.Message = "Mail Sent.";
                }
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
}
