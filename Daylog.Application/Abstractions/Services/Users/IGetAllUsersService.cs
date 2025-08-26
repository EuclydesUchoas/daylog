using Daylog.Application.Dtos.Users.Request;
using Daylog.Domain.Entities.Users;

namespace Daylog.Application.Abstractions.Services.Users;

public interface IGetAllUsersService : IService<GetAllUsersRequestDto, IEnumerable<User>>;
