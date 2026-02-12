using LocationSystem.Application.Features.DentalOffices.Commands.CreateDentalOffice;
using LocationSystem.Application.Features.DentalOffices.Commands.DeleteDentalOffice;
using LocationSystem.Application.Features.DentalOffices.Commands.UpdateDentalOffice;
using LocationSystem.Application.Features.DentalOffices.Queries.GetDentalOfficesDetail;
using LocationSystem.Application.Features.DentalOffices.Queries.GetDetalOfficesList;
using LocationSystem.Application.Utilities;
using LocationSystem.Api.Filters;
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
        [PermissionAuthorize("dental-office:create")]
        public async Task<IActionResult> Post([FromBody]CreateDentalOffceDetailDto dto)
        {
            var command = new CreateDentalOfficesCommand { Name = dto.Name};
            await _mediator.Send(command);
            return Ok();
        }
        [HttpGet("{id}")]
        [PermissionAuthorize("dental-office:view")]
        public async Task<IActionResult> Get(Guid id)
        {
            var command = new GetDentalOffcesDetailQuery() { Id = id };
            var model =await _mediator.Send(command);
            return Ok(model);
        }
        [HttpGet]
        [PermissionAuthorize("dental-office:view")]
        public async Task<IActionResult> GetDentalOffices([FromQuery]DentalOfficeListFilter filter)
        {
            var command = new GetDetalOfficesListQuery()
            {
                Page = filter.Page,
                PageSize = filter.PageSize,
                keyWord = filter.keyWord
            };
            var model = await _mediator.Send(command);
            return Ok(model);
        }
        [HttpPut("{id}")]
        [PermissionAuthorize("dental-office:edit")]
        public async Task<IActionResult> Put(Guid id,[FromBody]UpdateDetalOfficeDto updateDetalOfficeDto)
        {
            var command = new UpdateDetalOfficeCommand { Id = id,Name = updateDetalOfficeDto.Name };
            await _mediator.Send(command);
            return NoContent();
        }
        [HttpDelete("{id}")]
        [PermissionAuthorize("dental-office:delete")]
        public async Task<IActionResult> Delete(Guid id) 
        {
            var command = new DeleteDentalOfficeCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
