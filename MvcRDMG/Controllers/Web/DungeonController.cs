using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MvcRDMG.Controllers.Web
{
    public class DungeonController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}
