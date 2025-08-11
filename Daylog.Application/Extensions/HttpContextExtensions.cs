using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Daylog.Application.Extensions;

public static class HttpContextExtensions
{
    public static int? GetUserId(this HttpContext httpContext)
    {
        string? claimValue = httpContext.User.FindFirstValue(ClaimTypes.PrimarySid);

        int? userId = !string.IsNullOrWhiteSpace(claimValue) && int.TryParse(claimValue, out int id) && id > 0
            ? id : null;

        return userId;
    }
}
