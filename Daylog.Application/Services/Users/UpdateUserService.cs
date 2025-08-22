using Daylog.Application.Abstractions.Data;
using Daylog.Application.Abstractions.Services.Users;
using Daylog.Application.Dtos.Users.Request;
using Daylog.Application.Mappings.Users;
using Daylog.Domain.Entities.Users;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Daylog.Application.Services.Users;

public sealed class UpdateUserService(
    IValidator<UpdateUserRequestDto> validator,
    IAppDbContext appDbContext
    ) : IUpdateUserService
{
    public async Task<User> HandleAsync(UpdateUserRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        await validator.ValidateAndThrowAsync(requestDto, cancellationToken);

        var user = requestDto.ToDomain();
        ArgumentNullException.ThrowIfNull(user, nameof(user));

        var userDb = await appDbContext.Users // Change Tracking is required for updates
            .Include(x => x.UserDepartments)
            .FirstOrDefaultAsync(x => x.Id == user.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"User with ID {user.Id} not found.");

        userDb.Update(user);

        await appDbContext.SaveChangesAsync(cancellationToken);

        return userDb;
    }
}
