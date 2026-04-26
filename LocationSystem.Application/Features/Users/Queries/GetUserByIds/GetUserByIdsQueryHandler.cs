using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.GrapqLDTOs.Users;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Users.Queries.GetUserByIds
{
    public class GetUserByIdsQueryHandler : IRequestHandler<GetUserByIdsQuery, Dictionary<Guid, UserGraphqLDto>>
    {
        private readonly IUserRepository repository;
        public GetUserByIdsQueryHandler(IUserRepository userRepository)
        {
            repository = userRepository;
        }
        public async Task<Dictionary<Guid, UserGraphqLDto>> Handle(GetUserByIdsQuery request)
        {
            return await repository.GetUserByIds(request.Ids);
        }
    }
}
