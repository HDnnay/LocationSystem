using LocationSystem.Application.Features.DentalOffices.Commands.CreateDentalOffice;
using LocationSystem.Application.Features.DentalOffices.Queries.GetDentalOfficesDetail;
using LocationSystem.Application.Features.DentalOffices.Queries.GetDetalOfficesList;
using LocationSystem.Application.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LocationSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DentalOfficesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public DentalOfficesController(IMediator mediator) 
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateDentalOffceDetailDto dto)
        {
            var command = new CreateDentalOfficesCommand { Name = dto.Name };
            await _mediator.Send(command);
            return Ok();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var command = new GetDentalOffcesDetailQuery() { Id = id };
            var model =await _mediator.Send(command);
            return Ok(model);
        }
        [HttpGet]
        public async Task<IActionResult> GetDentalOffices()
        {
            var command = new GetDetalOfficesListQuery();
            var model = await _mediator.Send(command);
            return Ok(model);
        }
    }
}
