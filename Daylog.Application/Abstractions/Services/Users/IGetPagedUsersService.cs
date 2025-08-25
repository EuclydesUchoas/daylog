using Daylog.Application.Dtos.Users.Request;
using Daylog.Application.Models;
using Daylog.Domain.Entities.Users;

namespace Daylog.Application.Abstractions.Services.Users;

public interface IGetPagedUsersService : IService<GetPagedUsersRequestDto, PagedResponseModel<User>>;
