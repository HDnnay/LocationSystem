using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.GrapqLDTOs.Menus;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Menus.Queries.GetMenusByIds
{
    public class GetMenusByIdsQueryHandler : IRequestHandler<GetMenusByIdsQuery, Dictionary<Guid, MenuGraphqLDto>>
    {
        private readonly IMenuRepository _repository;

        public GetMenusByIdsQueryHandler(IMenuRepository repository)
        {
            _repository = repository;
        }

        public async Task<Dictionary<Guid, MenuGraphqLDto>> Handle(GetMenusByIdsQuery request)
        {
            return await _repository.GetMenusByIdsAsync(request.Ids);
        }
    }
}