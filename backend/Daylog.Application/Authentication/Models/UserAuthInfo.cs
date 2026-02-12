using Daylog.Application.UserProfiles.Dtos.Response;

namespace Daylog.Application.Authentication.Models;

public sealed record UserAuthInfo(
    Guid Id,
    string Email,
    string Name,
    UserProfileResponseDto Profile
    );
