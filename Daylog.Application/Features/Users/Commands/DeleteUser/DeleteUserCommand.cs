using MediatR;

namespace Daylog.Application.Features.Users.Commands.DeleteUser;

public sealed record DeleteUserCommand(
    int UserId
    ) : IRequest<bool>;
