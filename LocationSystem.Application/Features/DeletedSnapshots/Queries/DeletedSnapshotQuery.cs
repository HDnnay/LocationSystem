using LocationSystem.Application.Dtos.DeletedSnapshots;
using LocationSystem.Application.Utilities.Common;

namespace LocationSystem.Application.Features.DeletedSnapshots.Queries
{
    public class DeletedSnapshotQuery : PageRequest, Utilities.IRequest<PageResult<DeletedSnapshotDto>>
    {

    }
}
