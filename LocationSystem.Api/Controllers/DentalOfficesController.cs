using LocationSystem.Application.Features.DentalOffices.Commands.CreateDentalOffice;
using LocationSystem.Application.Features.DentalOffices.Commands.DeleteDentalOffice;
using LocationSystem.Application.Features.DentalOffices.Commands.UpdateDentalOffice;
using LocationSystem.Application.Features.DentalOffices.Queries.GetDentalOfficesDetail;
using LocationSystem.Application.Features.DentalOffices.Queries.GetDetalOfficesList;
using LocationSystem.Application.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Post([FromBody]CreateDentalOffceDetailDto dto)
        {
            var command = new CreateDentalOfficesCommand { Name = dto.Name};
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
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id,[FromBody]UpdateDetalOfficeDto updateDetalOfficeDto)
        {
            var command = new UpdateDetalOfficeCommand { Id = id,Name = updateDetalOfficeDto.Name };    
            await _mediator.Send(command);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id) 
        {
            var command = new DeleteDentalOfficeCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
