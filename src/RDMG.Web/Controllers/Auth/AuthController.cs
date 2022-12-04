using AutoMapper;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using RDMG.Core.Abstractions.Services;
using RDMG.Core.Abstractions.Services.Models;
using RDMG.Web.Models.Auth;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RDMG.Web.Controllers.Auth;

public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;
    public AuthController(IAuthService authService, IMapper mapper)
    {
        _authService = authService;
        _mapper = mapper;
    }
    public IActionResult Login()
    {
        if (User.Identity is { IsAuthenticated: true })
        {
            return RedirectToAction("Index", "Dungeon");
        }
        return View();
    }
    [HttpPost]
    public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
    {
        if (ModelState.IsValid)
        {
            var user = await _authService.LoginAsync(_mapper.Map<UserModel>(model));

            if (user == null)
            {
                ModelState.AddModelError("", "Username or password incorrect");
                return View();
            }

            var claims = new List<Claim>
            {
                new(JwtClaimTypes.Name, user.Username),
                new(JwtClaimTypes.Id, user.Id.ToString()),
                new(JwtClaimTypes.Email, user.Email),
                new(JwtClaimTypes.Role, user.Role)
            };

            var userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme, JwtClaimTypes.Subject, JwtClaimTypes.Role);

            var authProperties = (await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme)).Properties;
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(userIdentity),
                authProperties);

            if (string.IsNullOrWhiteSpace(returnUrl))
                return RedirectToAction("Index", "Dungeon");
            return Redirect(returnUrl);
        }
        return View();
    }
    public async Task<ActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Forbidden()
    {
        return View();
    }
}