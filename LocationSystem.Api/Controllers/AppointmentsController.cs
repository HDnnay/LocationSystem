using LocationSystem.Application.Features.Appointments.Commands.CreateAppointment;
using LocationSystem.Application.Features.Appointments.Commands.DeleteAppointment;
using LocationSystem.Application.Features.Appointments.Commands.UpdateAppointment;
using LocationSystem.Application.Features.Appointments.Queries.GetAppointmentDetail;
using LocationSystem.Application.Features.Appointments.Queries.GetAppointmentList;
using LocationSystem.Application.Utilities;
using LocationSystem.Application.Utilities.RabbitMQs;
using LocationSystem.Api.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LocationSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IRabbitMQService _rabbitMQService;
        public AppointmentsController(IMediator mediator, IRabbitMQService rabbitMQService) 
        {
            _mediator = mediator;
            _rabbitMQService = rabbitMQService;
        }
        // GET: api/<AppointmentsController>
        [HttpGet]
        [PermissionAuthorize("appointment:view")]
        public async Task<IActionResult> Get([FromQuery]AppointmentListFilter filter)
        {
            var command = new GetAppointmentListQuery()
            {
                Page = filter.Page,
                PageSize = filter.PageSize,
                keyWord = filter.keyWord
            };
           var model =await _mediator.Send(command);
            return Ok(model);
        }

        // GET api/<AppointmentsController>/5
        [HttpGet("{id}")]
        [PermissionAuthorize("appointment:view")]
        public async Task<IActionResult> Get(Guid id)
        {
            var command = new GetAppointmentDetailQuery { Id = id };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // POST api/<AppointmentsController>
        [HttpPost]
        [PermissionAuthorize("appointment:create")]
        public async Task<IActionResult> Post([FromBody] CreateAppointmentDto model)
        {
            var command = new CreateAppointmentCommand() 
            {
                PatientId = model.PatientId,
                DentistId = model.DentistId,
                DentalOfficeId = model.DentalOfficeId,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
            };
            var result = await _mediator.Send(command);
            await _rabbitMQService.PublishAsync(exchange:"",routingKey:"my_queue",message:"appointment_created");
            return Ok(result);
        }

        // PUT api/<AppointmentsController>/5
        [HttpPut("{id}")]
        [PermissionAuthorize("appointment:edit")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateAppointmentDto model)
        {
            var command = new UpdateAppointmentCommand
            {
                Id = id,
                DentalOfficeId = model.DentalOfficeId,
                DentistId = model.DentistId,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Status = model.Status,
            };
            await _mediator.Send(command);
            return Ok();
        }

        // DELETE api/<AppointmentsController>/5
        [HttpDelete("{id}")]
        [PermissionAuthorize("appointment:delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteAppointmentCommand { Id = id };
            await _mediator.Send(command);
            return Ok();
        }
    }
}
