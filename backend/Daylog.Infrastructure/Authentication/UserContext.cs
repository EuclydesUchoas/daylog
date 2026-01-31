using Daylog.Application.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;

namespace Daylog.Infrastructure.Authentication;

public sealed class UserContext(
    IHttpContextAccessor _httpContextAccessor
    ) : IUserContext
{
    public Guid? UserId
        => _httpContextAccessor.HttpContext?.User.GetUserId();
}
