using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MvcRDMG.Controllers.Web
{
    [Authorize]
    public class LoadDungeonController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}