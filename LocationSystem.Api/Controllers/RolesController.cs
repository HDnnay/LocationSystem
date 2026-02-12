using LocationSystem.Application.Dtos;
using LocationSystem.Application.Features.Roles.Commands.CreateRole;
using LocationSystem.Application.Features.Roles.Commands.DeleteRole;
using LocationSystem.Application.Features.Roles.Commands.DisableRole;
using LocationSystem.Application.Features.Roles.Commands.EnableRole;
using LocationSystem.Application.Features.Roles.Commands.UpdateRole;
using LocationSystem.Application.Features.Roles.Queries.GetRoleDetail;
using LocationSystem.Application.Features.Roles.Queries.GetRoleList;
using LocationSystem.Application.Utilities;
using LocationSystem.Api.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LocationSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RolesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [PermissionAuthorize("role:view")]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                var query = new GetRoleListQuery();
                var roles = await _mediator.Send(query);
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [PermissionAuthorize("role:view")]
        public async Task<IActionResult> GetRole(Guid id)
        {
            try
            {
                var query = new GetRoleDetailQuery { RoleId = id };
                var role = await _mediator.Send(query);
                return Ok(role);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [PermissionAuthorize("role:create")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto roleDto)
        {
            try
            {
                var command = new CreateRoleCommand { RoleDto = roleDto };
                var createdRole = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetRole), new { id = createdRole.Id }, createdRole);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [PermissionAuthorize("role:edit")]
        public async Task<IActionResult> UpdateRole(Guid id, [FromBody] UpdateRoleDto roleDto)
        {
            try
            {
                var command = new UpdateRoleCommand { RoleId = id, RoleDto = roleDto };
                var updatedRole = await _mediator.Send(command);
                return Ok(updatedRole);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [PermissionAuthorize("role:delete")]
        public async Task<IActionResult> DeleteRole(Guid id)
        {
            try
            {
                var command = new DeleteRoleCommand { RoleId = id };
                var result = await _mediator.Send(command);
                return Ok(new { success = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id}/disable")]
        [PermissionAuthorize("role:edit")]
        public async Task<IActionResult> DisableRole(Guid id)
        {
            try
            {
                var command = new DisableRoleCommand { RoleId = id };
                var result = await _mediator.Send(command);
                return Ok(new { success = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id}/enable")]
        [PermissionAuthorize("role:edit")]
        public async Task<IActionResult> EnableRole(Guid id)
        {
            try
            {
                var command = new EnableRoleCommand { RoleId = id };
                var result = await _mediator.Send(command);
                return Ok(new { success = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}