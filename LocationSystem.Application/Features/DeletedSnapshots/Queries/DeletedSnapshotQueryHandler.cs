using LocationSystem.Application.Dtos.DeletedSnapshots;
using LocationSystem.Application.Utilities;
using LocationSystem.Application.Utilities.Common;

namespace LocationSystem.Application.Features.DeletedSnapshots.Queries
{
    public class DeletedSnapshotQueryHandler : IRequestHandler<DeletedSnapshotQuery, PageResult<DeleteSnapshotDto>>
    {
        public DeletedSnapshotQueryHandler(IS)
        {

        }
        public Task<PageResult<DeleteSnapshotDto>> Handle(DeletedSnapshotQuery request)
        {
            throw new NotImplementedException();
        }
    }
}
