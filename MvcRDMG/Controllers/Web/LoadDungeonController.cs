using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MvcRDMG.Controllers.Web
{
    public class LoadDungeonController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}