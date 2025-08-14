using Daylog.Application.Abstractions.Data;
using Daylog.Application.Dtos.Users;
using Daylog.Application.Mappings.Users;
using Daylog.Domain.Entities.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Daylog.Application.Features.Users.Commands.UpdateUser;

public sealed class UpdateUserCommandHandler(
    IAppDbContext _appDbContext
    ) : IRequestHandler<UpdateUserCommand, User>
{
    public async Task<User> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = request.ToDomain();
        ArgumentNullException.ThrowIfNull(user, nameof(user));

        var userDb = await _appDbContext.Users // Change Tracking is required for updates
            .FirstOrDefaultAsync(u => u.Id == user.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"User with ID {user.Id} not found.");

        userDb.Update(user);

        await _appDbContext.SaveChangesAsync(cancellationToken);

        return userDb;

        bool userExists = await _appDbContext.Users.AsNoTracking()
            .AnyAsync(u => u.Id == user.Id, cancellationToken);

        if (!userExists)
        {
            throw new KeyNotFoundException($"User with ID {user.Id} not found.");
        }

        var userBase = new User(user.Id);

        _appDbContext.Users.Attach(userBase);

        userBase.Update(user);

        await _appDbContext.SaveChangesAsync(cancellationToken);

        return userBase;
    }
}
