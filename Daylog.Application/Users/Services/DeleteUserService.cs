using Daylog.Application.Abstractions.Data;
using Daylog.Application.Common.Results;
using Daylog.Application.Users.Dtos.Request;
using Daylog.Application.Users.Results;
using Daylog.Application.Users.Services.Contracts;
using Daylog.Domain.Users;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Daylog.Application.Users.Services;

public sealed class DeleteUserService(
    IValidator<DeleteUserRequestDto> validator,
    IAppDbContext appDbContext
    ) : IDeleteUserService
{
    public async Task<Result> HandleAsync(DeleteUserRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        if (requestDto is null)
        {
            return Result.Failure(ResultError.NullData);
        }

        var validationResult = await validator.ValidateAsync(requestDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Result.Failure(ResultError.Validation(validationResult.Errors));
        }

        var user = await appDbContext.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == new UserId(requestDto.Id), cancellationToken);

        if (user is null)
        {
            return Result.Failure(UserResultErrors.NotFound(requestDto.Id));
        }

        appDbContext.Users.Remove(user);

        int deletedUsers = await appDbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
