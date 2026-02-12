using Daylog.Application.Abstractions.Dtos;
using Daylog.Application.UserProfiles.Dtos.Response;
using System.Diagnostics.CodeAnalysis;

namespace Daylog.Application.Authentication.Dtos.Response;

public sealed class LoginUserInfoResponseDto : IResponseDto
{
    public required Guid Id { get; init; }

    public required string Name { get; init; }

    public required string Email { get; init; }

    public required UserProfileResponseDto Profile { get; init; }

    public LoginUserInfoResponseDto() { }

    [SetsRequiredMembers]
    public LoginUserInfoResponseDto(Guid id, string name, string email, UserProfileResponseDto profile)
    {
        Id = id;
        Name = name;
        Email = email;
        Profile = profile;
    }
}
