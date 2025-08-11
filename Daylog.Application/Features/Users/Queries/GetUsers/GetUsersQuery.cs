using Daylog.Domain.Entities.Users;
using MediatR;

namespace Daylog.Application.Features.Users.Queries.GetUsers;

public sealed record GetUsersQuery : IRequest<IEnumerable<User>>;
