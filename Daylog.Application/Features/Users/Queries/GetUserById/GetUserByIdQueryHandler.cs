using Daylog.Application.Abstractions.Data;
using Daylog.Domain.Entities.Users;
using FluentValidation;
using MediatR;

namespace Daylog.Application.Features.Users.Queries.GetUserById;

public sealed class GetUserByIdQueryHandler(
    IValidator<GetUserByIdQuery> _validator,
    IAppDbContext _appDbContext
    ) : IRequestHandler<GetUserByIdQuery, User?>
{
    public async Task<User?> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(query, cancellationToken);

        var user = await _appDbContext.Users.FindAsync([query.UserId], cancellationToken);

        return user;
    }
}
