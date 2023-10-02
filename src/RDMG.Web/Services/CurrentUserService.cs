using IdentityModel;
using Microsoft.AspNetCore.Http;
using RDMG.Core.Abstractions.Services;
using System.Security.Claims;

namespace RDMG.Web.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int GetUserIdAsInt()
    {
        return int.TryParse(UserId, out var result) ? result : -1;
    }

    public string UserId => _httpContextAccessor.HttpContext?
        .User
        .FindFirstValue(JwtClaimTypes.Id);

    public string UserName => _httpContextAccessor.HttpContext?
        .User
        .FindFirstValue(JwtClaimTypes.PreferredUserName);
}