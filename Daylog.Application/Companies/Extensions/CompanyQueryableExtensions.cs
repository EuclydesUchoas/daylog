using Daylog.Application.Common.Dtos.Response;
using Daylog.Application.Companies.Dtos.Response;
using Daylog.Domain.Companies;
using Microsoft.EntityFrameworkCore;

namespace Daylog.Application.Companies.Extensions;

public static class CompanyQueryableExtensions
{
    public static IQueryable<CompanyResponseDto> SelectCompanyResponseDto(this IQueryable<Company> companies)
    {
        return companies.Select(x => new CompanyResponseDto
        {
            Id = x.Id,
            Name = x.Name,
            Users = x.Users.Select(x2 => new CompanyUserResponseDto
            {
                UserId = x2.UserId,
                UserName = x2.User.Name,
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
