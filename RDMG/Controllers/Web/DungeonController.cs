using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RDMG.Controllers.Web
{
    [Authorize]
    public class DungeonController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
