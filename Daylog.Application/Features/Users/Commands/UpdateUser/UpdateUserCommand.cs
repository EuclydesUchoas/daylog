using Daylog.Domain.Entities.Users;
using MediatR;

namespace Daylog.Application.Features.Users.Commands.UpdateUser;

public sealed record UpdateUserCommand(
    int Id,
    string Name,
    string Email,
    int Profile
    ) : IRequest<User>;
