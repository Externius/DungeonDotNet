using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MvcRDMG.Models;
using MvcRDMG.Services;
using MvcRDMG.ViewModels;

namespace MvcRDMG.Controllers.Web
{
    public class HomeController : Controller
    {
        private IMailService _mailService;

        public HomeController(IMailService service)
        {
            _mailService = service;
        }

        public IActionResult Index()
        {
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

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
