using Daylog.Application.Common.Dtos.Response;
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
            Companies = x.Companies.Select(x2 => new UserCompanyResponseDto
            {
                CompanyId = x2.CompanyId,
                CompanyName = x2.Company.Name,
                CreatedInfo = new CreatedInfoResponseDto
                {
                    CreatedAt = x2.CreatedAt,
                    CreatedByUserId = x2.CreatedByUserId,
                    CreatedByUserName = x2.CreatedByUser!.Name
                },
                UpdatedInfo = new UpdatedInfoResponseDto
                {
                    UpdatedAt = x2.UpdatedAt,
                    UpdatedByUserId = x2.UpdatedByUserId,
                    UpdatedByUserName = x2.UpdatedByUser!.Name
                }
            }),
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
