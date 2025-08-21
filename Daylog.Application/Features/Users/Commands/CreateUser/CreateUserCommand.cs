using Daylog.Domain.Entities.Users;
using MediatR;

namespace Daylog.Application.Features.Users.Commands.CreateUser;

public sealed record CreateUserCommand(
    string Name,
    string Email,
    string Password,
    int Profile,
    ICollection<CreateUserDepartmentCommand> UserDepartments
    ) : IRequest<User>;
