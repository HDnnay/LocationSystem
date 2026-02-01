using LocationSystem.Application.Features.RentHousies.Command.CreateRentHose;
using LocationSystem.Application.Features.RentHousies.Queries.GetRentHouseList;
using LocationSystem.Application.Features.RentHousies.Queries.QueryRentHouseList;
using LocationSystem.Application.Utilities;
using LocationSystem.Application.Utilities.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LocationSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentHouseController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RentHouseController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> Get(GetRentHouseListFilter request)
        {
            var query = new GetRentHouseListQuery()
            {
                Page = request.Page,
                PageSize = request.PageSize,
                keyWord = request.keyWord
            };
            var data = await _mediator.Send(query);
            return Ok(data);
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateRentHouseDto model)
        {
            var command = new CreateRentHouseCommand() { Model=model };
            await _mediator.Send(command);
            return Ok();
        }

        
    }
}
