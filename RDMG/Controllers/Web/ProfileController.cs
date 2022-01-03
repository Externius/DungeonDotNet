using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RDMG.Core.Abstractions.Services;
using RDMG.Core.Abstractions.Services.Models;
using RDMG.Helpers;
using RDMG.Models.Profile;
using System;
using System.Threading.Tasks;

namespace RDMG.Controllers.Web
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public ProfileController(IUserService userService, IMapper mapper, ILogger<ProfileController> logger)
        {
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _userService.GetAsync(UserHelper.GetUserId(User.Claims));
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
                    _logger.LogError(ex, "Error changing password.");
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(model);
        }
    }
}
