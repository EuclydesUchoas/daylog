using MediatR;

namespace Daylog.Application.Features.Users.Commands.UpdateUser;

public sealed record UpdateUserDepartmentCommand(
    int DepartmentId
    ) : IRequest;
