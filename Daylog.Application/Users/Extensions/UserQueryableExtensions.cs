using Daylog.Application.Common.Dtos.Response;
using Daylog.Application.UserProfiles.Dtos.Response;
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
            Profile = new UserProfileResponseDto
            {
                Id = x.Profile.Id,
                Name = EF.Property<string>(x.Profile, $"{nameof(UserProfile.Name)}_{culture}")
            },
            CreatedInfo = new CreatedInfoResponseDto
            {
                CreatedAt = x.CreatedAt,
                CreatedByUserId = x.CreatedByUserId,
                CreatedByUserName = x.CreatedByUser!.Name
            },
            UpdatedInfo = new UpdatedInfoResponseDto
            {
                UpdatedAt = x.UpdatedAt,
                UpdatedByUserId = x.UpdatedByUserId,
                UpdatedByUserName = x.UpdatedByUser!.Name
            },
            DeletedInfo = new DeletedInfoResponseDto
            {
                IsDeleted = x.IsDeleted,
                DeletedAt = x.DeletedAt,
                DeletedByUserId = x.DeletedByUserId,
                DeletedByUserName = x.DeletedByUser!.Name,
            }
        });
    }
}
