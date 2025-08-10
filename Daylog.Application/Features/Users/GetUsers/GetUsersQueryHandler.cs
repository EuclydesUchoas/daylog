using Daylog.Application.Abstractions.Messaging;
using Daylog.Domain.Entities.Users;

namespace Daylog.Application.Features.Users.GetUsers;

public sealed class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, IEnumerable<User>>
{
    public async Task<IEnumerable<User>> Handle(GetUsersQuery query, CancellationToken cancellationToken)
    {
        var users = await Task.FromResult(new List<User>
        {
            new User { Id = 1, Name = "John Doe", Email = "john.doe@gmail.com", Password = "password123", Profile = 1, CreatedDate = DateTime.UtcNow },
            new User { Id = 2, Name = "Jane Smith", Email = "jane.smith@gmail.com", Password = "password456", Profile = 2, CreatedDate = DateTime.UtcNow },
        });

        return users;
    }
}
