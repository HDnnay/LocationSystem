using LocationSystem.Application.Features.DeletedSnapshots.Queries;
using LocationSystem.Application.Utilities;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace LocationSystem.Api.Controllers
{
    [Route("api/deleted")]
    [ApiController]
    public class DeletedSnapshotController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DeletedSnapshotController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("history")]

        public async Task<IActionResult> History([FromQuery] GetDeletedSnapshotListFilter filter)
        {
            var query = filter.Adapt<DeletedSnapshotQuery>();
            var data = await _mediator.Send(query);
            return Ok(data);
        }
    }
}
