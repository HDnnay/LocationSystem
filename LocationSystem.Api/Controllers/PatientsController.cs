using LocationSystem.Application.Features.DentalOffices.Queries.GetDetalOfficesList;
using LocationSystem.Application.Features.Patients.Command.CreatePatient;
using LocationSystem.Application.Features.Patients.Command.DeletePatient;
using LocationSystem.Application.Features.Patients.Command.UpdatePatient;
using LocationSystem.Application.Features.Patients.Queries.GetPatienDetail;
using LocationSystem.Application.Features.Patients.Queries.GetPatienList;
using LocationSystem.Application.Utilities;
using LocationSystem.Application.Utilities.RabbitMQs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace LocationSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IRabbitMQService _rabbitMQService;

        public PatientsController(IMediator mediator, IRabbitMQService rabbitMQService) 
        {
            _mediator = mediator;
            _rabbitMQService = rabbitMQService;
        }
        [HttpPost]
        public async Task<IActionResult> Post(CreatePatientDto model)
        {
            var command = new CreatePatientCommand() { Name = model.Name ,Email = model.Email};
            var result = await _mediator.Send(command);
            await _rabbitMQService.PublishAsync(exchange: "", routingKey: "my_queue", message: "Patients_created");

            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var command = new GetPatienDetailQuery() { PatientId = id };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]PatiensListFilterDto dto)
        {
            var command = new GetPatienListQuery() { Page = dto.Page, PageSize = dto.PageSize,keyWord=dto.keyWord };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdatePatientDto dto)
        {
            var command = new UpdatePatientCommand()
            {
                Id = id,
                Name = dto.Name,
                Email = dto.Email,
            };
            await _mediator.Send(command);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeletePatientCommand { Id =id};
            await _mediator.Send(command);
            return Ok();
        }
    }
}
