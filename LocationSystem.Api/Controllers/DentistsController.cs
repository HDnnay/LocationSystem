using LocationSystem.Application.Features.Dentists.Commands.CreateDentist;
using LocationSystem.Application.Features.Dentists.Commands.DeleteDentist;
using LocationSystem.Application.Features.Dentists.Commands.UpdateDentist;
using LocationSystem.Application.Features.Dentists.Queries.GetDentistList;
using LocationSystem.Application.Utilities;
using LocationSystem.Application.Utilities.RabbitMQs;
using LocationSystem.Api.Filters;
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
        private readonly IRabbitMQService _rabbitMQService;

        public DentistsController(IMediator mediator,IRabbitMQService rabbitMQService) 
        {
            _mediator = mediator;
            _rabbitMQService = rabbitMQService;
        }

        // GET api/<DentistsController>/5
        [HttpGet]
        [PermissionAuthorize("dentist:view")]
        public async Task<IActionResult> Get([FromQuery]DentistListFilterDto model)
        {
            var command = new GetDentistListQuery()
            {
                Page = model.Page,
                PageSize = model.PageSize,
                keyWord = model.keyWord
            };
            var result = await _mediator.Send(command);
            await _rabbitMQService.PublishAsync(exchange: "", routingKey: "my_queue", message: "get list");

            return Ok(result);
        }

        // POST api/<DentistsController>
        [HttpPost]
        [PermissionAuthorize("dentist:create")]
        public async Task<IActionResult> Post([FromBody] CreateDentistDto model)
        {
            
            var command = new CreateDentistCommand() { Email = model.Email ,Name =model.Name};
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // PUT api/<DentistsController>/5
        [HttpPut("{id}")]
        [PermissionAuthorize("dentist:edit")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateDentistDto model)
        {
            var command = new UpdateDentistCommand() { Id = id, Name = model.Name, Email = model.Email };
            await _mediator.Send(command);
            return Ok();
        }
            // DELETE api/<DentistsController>/5
        [HttpDelete("{id}")]
        [PermissionAuthorize("dentist:delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteDentistCommand() { Id = id };
            await _mediator.Send(command);
            return Ok();
        }
    }
}
