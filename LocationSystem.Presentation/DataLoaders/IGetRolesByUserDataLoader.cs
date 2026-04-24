using HotChocolate.Fetching;
using LocationSystem.Application.GrapqLDTOs.Roles;

namespace LocationSystem.Presentation.DataLoaders
{
    public interface IGetRolesByUserDataLoader : IDataLoader<Guid, List<RoleGraphqLDto>>
    {
    }
}