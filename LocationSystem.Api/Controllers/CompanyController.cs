using LocationSystem.Application.Features.Appointments.Queries.GetAppointmentList;
using LocationSystem.Application.Features.Companys.Queries.ReadConpany;
using LocationSystem.Application.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LocationSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CompanyController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] CompanyFilter request)
        {
            var command = new ReadConpanyQuery()
            {
                Page = request.Page,
                PageSize = request.PageSize,
                keyWord = request.keyWord
            };
            var model = await _mediator.Send(command);
            return Ok(model);
        }
    }
}
