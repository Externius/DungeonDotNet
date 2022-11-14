using IdentityModel;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace RDMG.Helpers;

public static class UserHelper
{
    public static int GetUserId(IEnumerable<Claim> claims)
    {
        return int.Parse(claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Id)?.Value ?? string.Empty);
    }

    public static string GetUserName(IEnumerable<Claim> claims)
    {
        return claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Name)?.Value;
    }
}