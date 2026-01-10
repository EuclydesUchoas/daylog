using Daylog.Application.Users.Dtos.Response;
using Daylog.Domain.UserProfiles;
using Daylog.Domain.Users;
using Daylog.Shared.Core.Constants;
using Microsoft.EntityFrameworkCore;

namespace Daylog.Application.Users.Extensions;

public static class UserQueryableExtensions
{
    public static IQueryable<UserResponseDto> SelectUserResponseDto(this IQueryable<User> users)
    {
        string culture = Cultures.CurrentCultureSnakeCase;

        return users.Select(x => new UserResponseDto
        {
            Id = x.Id,
            Name = x.Name,
            Email = x.Email,
            ProfileId = x.ProfileId,
            ProfileName = EF.Property<string>(x.Profile, $"{nameof(UserProfile.Name)}_{culture}"),
            CreatedAt = x.CreatedAt,
            CreatedByUserId = x.CreatedByUserId,
            UpdatedAt = x.UpdatedAt,
            UpdatedByUserId = x.UpdatedByUserId,
            IsDeleted = x.IsDeleted,
            DeletedAt = x.DeletedAt,
            DeletedByUserId = x.DeletedByUserId
        });
    }
}
