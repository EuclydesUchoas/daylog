using MediatR;

namespace Daylog.Application.Features.Users.Commands.CreateUser;

public sealed record CreateUserDepartmentCommand(
    int DepartmentId
    ) : IRequest;
