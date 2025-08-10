using Daylog.Application.Abstractions.Messaging;
using Daylog.Domain.Entities.Users;

namespace Daylog.Application.Features.Users.GetUsers;

public sealed record GetUsersQuery : IQuery<IEnumerable<User>>;
