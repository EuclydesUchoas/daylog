using Daylog.Application.Abstractions.Data;
using Daylog.Application.Mappings.Users;
using Daylog.Domain.Entities.Users;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Daylog.Application.Features.Users.Commands.UpdateUser;

public sealed class UpdateUserCommandHandler(
    IValidator<UpdateUserCommand> _validator,
    IAppDbContext _appDbContext
    ) : IRequestHandler<UpdateUserCommand, User>
{
    public async Task<User> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var user = request.ToDomain();

        ArgumentNullException.ThrowIfNull(user, nameof(user));

        var userDb = await _appDbContext.Users // Change Tracking is required for updates
            .Include(x => x.UserDepartments)
            .FirstOrDefaultAsync(u => u.Id == user.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"User with ID {user.Id} not found.");

        userDb.Update(user);

        await _appDbContext.SaveChangesAsync(cancellationToken);

        return userDb;
    }
}
