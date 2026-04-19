using LocationSystem.Application.Dtos.DeletedSnapshots;
using LocationSystem.Application.ISevices;
using LocationSystem.Application.Utilities;
using LocationSystem.Application.Utilities.Common;

namespace LocationSystem.Application.Features.DeletedSnapshots.Queries
{
    public class DeletedSnapshotQueryHandler : IRequestHandler<DeletedSnapshotQuery, PageResult<DeletedSnapshotDto>>
    {
        private readonly ISnapshotService _snapshotService;
        public DeletedSnapshotQueryHandler(ISnapshotService snapshotService)
        {
            _snapshotService = snapshotService;
        }
        public async Task<PageResult<DeletedSnapshotDto>> Handle(DeletedSnapshotQuery request)
        {
            var data = await _snapshotService.GetAllSnapshotsAsync(request.Page, request.PageSize);
            var result = new PageResult<DeletedSnapshotDto>
            {
                Items=data.Item2.ToList(),
                Total = data.Item1,
                CurrentPage = request.Page
            };
            return result;
        }
    }
}
