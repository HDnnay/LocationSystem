using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities.UserRolePermissions;

namespace LocationSystem.Application.Features.Users.Queries.GetUsers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IQueryable<User>>
    {
        private readonly IUserRepository _repository;
        public GetUsersQueryHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public Task<IQueryable<User>> Handle(GetUsersQuery request)
        {
            var result = _repository.Query();
            return Task.FromResult(result);
        }
    }
}
