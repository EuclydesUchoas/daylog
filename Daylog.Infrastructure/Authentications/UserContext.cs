using Daylog.Application.Abstractions.Authentications;
using Microsoft.AspNetCore.Http;

namespace Daylog.Infrastructure.Authentications;

public sealed class UserContext(
    IHttpContextAccessor _httpContextAccessor
    ) : IUserContext
{
    public int? UserId
        => _httpContextAccessor.HttpContext?.User.GetUserId();
}
