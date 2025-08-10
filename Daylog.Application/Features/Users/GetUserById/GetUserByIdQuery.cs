using Daylog.Application.Abstractions.Messaging;
using Daylog.Domain.Entities.Users;

namespace Daylog.Application.Features.Users.GetUserById;

public sealed record GetUserByIdQuery(
    int UserId
    ) : IQuery<User?>;
