using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcRDMG.Core.Abstractions.Services;
using MvcRDMG.Core.Abstractions.Services.Models;
using MvcRDMG.Models.User;
using System;
using System.Linq;

namespace MvcRDMG.Controllers.Web
{
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
        public IActionResult Index()
        {
            var list = _userService.List(null)
                        .Select(um => _mapper.Map<UserEditViewModel>(um))
                        .ToList();

            return View(new UserListViewModel
            {
                Details = list
            });
        }

        public IActionResult Edit(int id)
        {
            ViewData["Title"] = Resources.User.CreateTitle;
            var model = new UserEditViewModel();

            if (id != 0)
            {
                model = _mapper.Map<UserEditViewModel>(_userService.Get(id));
                model.Password = "";
                ViewData["Title"] = Resources.User.EditTitle;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.Id == 0)
                    {
                        _userService.Create(_mapper.Map<UserModel>(model));
                    }
                    else
                    {
                        var user = new UserModel();
                        _mapper.Map(model, user);
                        _userService.Update(user);
                    }
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            if (model.Id == 0)
                ViewData["Title"] = Resources.User.CreateTitle;
            else
                ViewData["Title"] = Resources.User.EditTitle;

            return View(model);
        }

        public ActionResult Delete(int id)
        {
            _userService.Delete(id);
            return RedirectToAction("Index");
        }

        public ActionResult Restore(int id)
        {
            _userService.Restore(id);
            return RedirectToAction("Index");
        }
    }
}