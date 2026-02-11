using LocationSystem.Application.Features.Menus.Commands.CreateMenu;
using LocationSystem.Application.Features.Menus.Commands.DeleteMenu;
using LocationSystem.Application.Features.Menus.Commands.UpdateMenu;
using LocationSystem.Application.Features.Menus.Queries.GetAllMenus;
using LocationSystem.Application.Features.Menus.Queries.GetMenuById;
using LocationSystem.Application.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace LocationSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenusController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MenusController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Menus
        [HttpGet]
        public async Task<IActionResult> GetMenus([FromQuery]GetAllMenusQuery query)
        {
            try
            {
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/Menus/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMenu(Guid id)
        {
            try
            {
                var query = new GetMenuByIdQuery { MenuId = id };
                var menu = await _mediator.Send(query);
                if (menu == null)
                {
                    return NotFound();
                }
                return Ok(menu);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/Menus
        [HttpPost]
        public async Task<IActionResult> CreateMenu([FromBody] CreateMenuCommand command)
        {
            try
            {
                var createdMenu = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetMenu), new { id = createdMenu.Id }, createdMenu);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/Menus/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMenu(Guid id, [FromBody] UpdateMenuCommand command)
        {
            try
            {
                command.Id = id;
                var updatedMenu = await _mediator.Send(command);
                return Ok(updatedMenu);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/Menus/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenu(Guid id)
        {
            try
            {
                var command = new DeleteMenuCommand { MenuId = id };
                await _mediator.Send(command);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
