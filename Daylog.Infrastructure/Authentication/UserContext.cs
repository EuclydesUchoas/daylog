using Daylog.Application.Abstractions.Authentication;
using Daylog.Domain.Users;
using Microsoft.AspNetCore.Http;

namespace Daylog.Infrastructure.Authentication;

public sealed class UserContext(
    IHttpContextAccessor _httpContextAccessor
    ) : IUserContext
{
    public UserId? UserId
        => _httpContextAccessor.HttpContext?.User.GetUserId();
}
