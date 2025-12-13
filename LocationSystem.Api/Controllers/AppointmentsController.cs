using LocationSystem.Application.Features.Appointments.Commands.CreateAppointment;
using LocationSystem.Application.Utilities;
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
        public AppointmentsController(IMediator mediator) 
        {
            _mediator = mediator;
        }
        // GET: api/<AppointmentsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AppointmentsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AppointmentsController>
        [HttpPost]
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
            return Ok(result);
        }

        // PUT api/<AppointmentsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AppointmentsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
