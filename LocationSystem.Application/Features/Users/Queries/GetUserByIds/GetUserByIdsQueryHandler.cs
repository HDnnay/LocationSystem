using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Dtos.Users;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Users.Queries.GetUserByIds
{
    public class GetUserByIdsQueryHandler : IRequestHandler<GetUserByIdsQuery, Dictionary<Guid, UserDto>>
    {
        private readonly IUserRepository repository;
        public GetUserByIdsQueryHandler(IUserRepository userRepository)
        {
            repository = userRepository;
        }
        public Task<Dictionary<Guid, UserDto>> Handle(GetUserByIdsQuery request)
        {
            throw new NotImplementedException();
        }
    }
}
