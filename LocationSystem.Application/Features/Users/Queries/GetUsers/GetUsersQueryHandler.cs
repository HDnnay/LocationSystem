using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.GrapqLDTOs.Users;
using LocationSystem.Application.Utilities;
using Mapster;

namespace LocationSystem.Application.Features.Users.Queries.GetUsers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IQueryable<UserGraphqLDto>>
    {
        private readonly IUserRepository _repository;
        public GetUsersQueryHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public Task<IQueryable<UserGraphqLDto>> Handle(GetUsersQuery request)
        {
            var result = _repository.Query().ProjectToType<UserGraphqLDto>();
            return Task.FromResult(result);
        }
    }
}
