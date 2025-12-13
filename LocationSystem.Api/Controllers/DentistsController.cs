using LocationSystem.Application.Features.Dentists.Commands.CreateDentist;
using LocationSystem.Application.Features.Dentists.Queries.GetDentistList;
using LocationSystem.Application.Utilities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

        // GET api/<DentistsController>/5
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]DentistListFilterDto model)
        {
            var command = new GetDentistListQuery()
            {
                Page = model.Page,
                PageSize = model.PageSize,
                keyWord = model.keyWord
            };
            var result = await _mediator.Send(command);
            return Ok(result);
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
