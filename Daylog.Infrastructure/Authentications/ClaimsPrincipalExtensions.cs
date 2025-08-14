using System.Security.Claims;

namespace Daylog.Infrastructure.Authentications;

public static class ClaimsPrincipalExtensions
{
    public static int? GetUserId(this ClaimsPrincipal? claimsPrincipal)
    {
        string? claimValue = claimsPrincipal?.FindFirstValue(ClaimTypes.PrimarySid);

        int? userId = !string.IsNullOrWhiteSpace(claimValue) && int.TryParse(claimValue, out int id) && id > 0
            ? id : null;

        return userId;
    }
}
