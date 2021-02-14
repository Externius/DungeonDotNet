using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcRDMG.Core.Abstractions.Services;
using MvcRDMG.Core.Abstractions.Services.Models;
using MvcRDMG.Models.User;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MvcRDMG.Controllers.Web
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public UserController(IUserService userService, IMapper mapper, ILogger<UserController> logger)
        {
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            var list = await _userService.ListAsync(null);


            return View(new UserListViewModel
            {
                Details = list.Select(um => _mapper.Map<UserEditViewModel>(um)).ToList()
            });
        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Title"] = Resources.User.CreateTitle;
            var model = new UserEditViewModel();

            if (id != 0)
            {
                model = _mapper.Map<UserEditViewModel>(await _userService.GetAsync(id));
                model.Password = "";
                ViewData["Title"] = Resources.User.EditTitle;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.Id == 0)
                    {
                        await _userService.CreateAsync(_mapper.Map<UserModel>(model));
                    }
                    else
                    {
                        var user = new UserModel();
                        _mapper.Map(model, user);
                        await _userService.UpdateAsync(user);
                    }
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error editing user.");
                    ModelState.AddModelError("", ex.Message);
                }
            }

            if (model.Id == 0)
                ViewData["Title"] = Resources.User.CreateTitle;
            else
                ViewData["Title"] = Resources.User.EditTitle;

            return View(model);
        }

        public async Task<ActionResult> Delete(int id)
        {
            await _userService.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Restore(int id)
        {
            await _userService.RestoreAsync(id);
            return RedirectToAction("Index");
        }
    }
}