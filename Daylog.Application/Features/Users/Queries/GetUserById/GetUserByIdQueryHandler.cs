using Daylog.Application.Abstractions.Data;
using Daylog.Domain.Entities.Users;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Daylog.Application.Features.Users.Queries.GetUserById;

public sealed class GetUserByIdQueryHandler(
    IValidator<GetUserByIdQuery> _validator,
    IAppDbContext _appDbContext
    ) : IRequestHandler<GetUserByIdQuery, User?>
{
    public async Task<User?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var user = await _appDbContext.Users.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        return user;
    }
}
