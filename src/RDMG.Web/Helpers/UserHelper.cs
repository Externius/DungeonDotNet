using IdentityModel;
using System.Security.Claims;

namespace RDMG.Web.Helpers;

public static class UserHelper
{
    public static string GetUserName(IEnumerable<Claim>? claims)
    {
        return claims?.FirstOrDefault(c => c.Type == JwtClaimTypes.Name)?.Value ?? string.Empty;
    }
}