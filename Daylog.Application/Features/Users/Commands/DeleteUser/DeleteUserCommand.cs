using MediatR;

namespace Daylog.Application.Features.Users.Commands.DeleteUser;

public sealed record DeleteUserCommand(
    int Id
    ) : IRequest<bool>;
