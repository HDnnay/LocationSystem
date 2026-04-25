using LocationSystem.Application.GrapqLDTOs.Menus;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Menus.Queries.GetMenusByIds
{
    public class GetMenusByIdsQuery : IRequest<Dictionary<Guid, MenuGraphqLDto>>
    {
        public IReadOnlyList<Guid> Ids { get; set; }
    }
}