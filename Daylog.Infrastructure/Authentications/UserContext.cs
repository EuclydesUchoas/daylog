using Daylog.Application.Abstractions.Authentications;
using Daylog.Domain.Users;
using Microsoft.AspNetCore.Http;

namespace Daylog.Infrastructure.Authentications;

public sealed class UserContext(
    IHttpContextAccessor _httpContextAccessor
    ) : IUserContext
{
    public UserId? UserId
        => _httpContextAccessor.HttpContext?.User.GetUserId();
}
