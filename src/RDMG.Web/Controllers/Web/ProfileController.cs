using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RDMG.Core.Abstractions.Services;
using RDMG.Core.Abstractions.Services.Models;
using RDMG.Web.Extensions;
using RDMG.Web.Models.Profile;

namespace RDMG.Web.Controllers.Web;

[Authorize]
public class ProfileController(IUserService userService,
    ICurrentUserService currentUserService,
    IMapper mapper,
    ILogger<ProfileController> logger) : Controller
{
    private readonly IUserService _userService = userService;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger _logger = logger;

    public async Task<IActionResult> Index()
    {
        var model = await _userService.GetAsync(_currentUserService.GetUserIdAsInt());
        return View(_mapper.Map<ProfileViewModel>(model));
    }


    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View(new ProfileChangePasswordModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ProfileChangePasswordModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _userService.ChangePasswordAsync(_mapper.Map<ChangePasswordModel>(model));
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                this.HandleException(ex, _logger, "Error changing password.");
            }
        }
        return View(model);
    }
}