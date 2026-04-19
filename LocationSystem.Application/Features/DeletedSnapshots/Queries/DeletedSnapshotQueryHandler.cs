using LocationSystem.Application.Dtos.DeletedSnapshots;
using LocationSystem.Application.ISevices;
using LocationSystem.Application.Utilities;
using LocationSystem.Application.Utilities.Common;

namespace LocationSystem.Application.Features.DeletedSnapshots.Queries
{
    public class DeletedSnapshotQueryHandler : IRequestHandler<DeletedSnapshotQuery, PageResult<DeleteSnapshotDto>>
    {
        private readonly ISnapshotService _snapshotService;
        public DeletedSnapshotQueryHandler(ISnapshotService snapshotService)
        {
            _snapshotService = snapshotService;
        }
        public Task<PageResult<DeleteSnapshotDto>> Handle(DeletedSnapshotQuery request)
        {
            throw new NotImplementedException();
        }
    }
}
