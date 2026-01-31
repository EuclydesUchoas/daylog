using System.Security.Claims;

namespace Daylog.Infrastructure.Authentication;

public static class ClaimsPrincipalExtensions
{
    public static Guid? GetUserId(this ClaimsPrincipal? claimsPrincipal)
    {
        string? claimValue = claimsPrincipal?.FindFirstValue(ClaimTypes.PrimarySid);

        Guid? userId = !string.IsNullOrWhiteSpace(claimValue) && Guid.TryParse(claimValue, out Guid id)
            ? id : null;

        return userId;
    }
}
