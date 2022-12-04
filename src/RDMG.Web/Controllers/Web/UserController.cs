using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RDMG.Core.Abstractions.Services;
using RDMG.Core.Abstractions.Services.Models;
using RDMG.Core.Domain;
using RDMG.Web.Models.User;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RDMG.Web.Controllers.Web;

[Authorize(Roles = Roles.Admin)]
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
            Details = list.Select(_mapper.Map<UserEditViewModel>).ToList()
        });
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new UserCreateViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserCreateViewModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _userService.CreateAsync(_mapper.Map<UserModel>(model));
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user.");
                ModelState.AddModelError("", ex.Message);
            }
        }
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var model = _mapper.Map<UserEditViewModel>(await _userService.GetAsync(id));
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
                var user = new UserModel();
                _mapper.Map(model, user);
                await _userService.UpdateAsync(user);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error editing user.");
                ModelState.AddModelError("", ex.Message);
            }
        }
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _userService.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user.");
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Restore(int id)
    {
        try
        {
            await _userService.RestoreAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error restoring user.");
        }
        return RedirectToAction("Index");
    }
}