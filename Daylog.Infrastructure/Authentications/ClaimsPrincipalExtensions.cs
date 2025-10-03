using Daylog.Domain.Users;
using System.Security.Claims;

namespace Daylog.Infrastructure.Authentications;

public static class ClaimsPrincipalExtensions
{
    public static UserId? GetUserId(this ClaimsPrincipal? claimsPrincipal)
    {
        string? claimValue = claimsPrincipal?.FindFirstValue(ClaimTypes.PrimarySid);

        UserId? userId = !string.IsNullOrWhiteSpace(claimValue) && Guid.TryParse(claimValue, out Guid id) && !id.Equals(Guid.Empty)
            ? UserId.Existing(id) : null;

        return userId;
    }
}
