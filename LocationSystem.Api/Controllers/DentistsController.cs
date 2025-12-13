using LocationSystem.Application.Features.Dentists.Commands.CreateDentist;
using LocationSystem.Application.Utilities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LocationSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DentistsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public DentistsController(IMediator mediator) 
        {
            _mediator = mediator;
        }
        // GET: api/<DentistsController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok();
        }

        // GET api/<DentistsController>/5
        [HttpGet("{id}")]
        public string Get(Guid id)
        {
            return "value";
        }

        // POST api/<DentistsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateDentistDto model)
        {
            
            var command = new CreateDentistCommand() { Email = model.Email ,Name =model.Name};
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // PUT api/<DentistsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<DentistsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
