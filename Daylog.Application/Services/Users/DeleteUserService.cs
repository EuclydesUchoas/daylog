using Daylog.Application.Abstractions.Data;
using Daylog.Application.Abstractions.Services.Users;
using Daylog.Application.Dtos.Users.Request;
using Daylog.Domain.Entities.Users;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Daylog.Application.Services.Users;

public sealed class DeleteUserService(
    IValidator<DeleteUserRequestDto> validator,
    IAppDbContext appDbContext
    ) : IDeleteUserService
{
    public async Task<bool> HandleAsync(DeleteUserRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        await validator.ValidateAndThrowAsync(requestDto, cancellationToken);

        var user = await appDbContext.Users.AsNoTracking()
            .Include(x => x.UserDepartments)
            .FirstOrDefaultAsync(u => u.Id == new UserId(requestDto.Id), cancellationToken);

        if (user is null)
        {
            return false;
        }

        appDbContext.Users.Remove(user);

        int deletedUsers = await appDbContext.SaveChangesAsync(cancellationToken);

        return deletedUsers > 0;
    }
}
