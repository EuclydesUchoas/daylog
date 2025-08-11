using Daylog.Domain.Entities.Users;
using MediatR;

namespace Daylog.Application.Features.Users.Queries.GetUserById;

public sealed record GetUserByIdQuery(
    int UserId
    ) : IRequest<User?>;
