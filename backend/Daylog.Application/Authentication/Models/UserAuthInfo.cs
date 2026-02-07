using Daylog.Domain.UserProfiles;

namespace Daylog.Application.Authentication.Models;

public sealed record UserAuthInfo(
    Guid Id,
    string Email,
    string Name,
    UserProfileEnum ProfileId
    );
