using LocationSystem.Application.Dtos;
using LocationSystem.Application.Features.Permissions.Commands.CreatePermission;
using LocationSystem.Application.Features.Permissions.Commands.DeletePermission;
using LocationSystem.Application.Features.Permissions.Commands.UpdatePermission;
using LocationSystem.Application.Features.Permissions.Queries.GetPermissionDetail;
using LocationSystem.Application.Features.Permissions.Queries.GetPermissionList;
using LocationSystem.Application.Features.Permissions.Queries.GetPermissionTree;
using LocationSystem.Application.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LocationSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PermissionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetPermissions()
        {
            try
            {
                var query = new GetPermissionListQuery();
                var permissions = await _mediator.Send(query);
                return Ok(permissions);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPermission(Guid id)
        {
            try
            {
                var query = new GetPermissionDetailQuery { PermissionId = id };
                var permission = await _mediator.Send(query);
                return Ok(permission);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePermission([FromBody] CreatePermissionDto permissionDto)
        {
            try
            {
                var command = new CreatePermissionCommand { PermissionDto = permissionDto };
                var createdPermission = await _mediator.Send(command);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePermission(Guid id, [FromBody] UpdatePermissionDto permissionDto)
        {
            try
            {
                var command = new UpdatePermissionCommand { PermissionId = id, PermissionDto = permissionDto };
                var updatedPermission = await _mediator.Send(command);
                return Ok(updatedPermission);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermission(Guid id)
        {
            try
            {
                var command = new DeletePermissionCommand { PermissionId = id };
                var result = await _mediator.Send(command);
                return Ok(new { success = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("tree")]
        public async Task<IActionResult> GetPermissionTree()
        {
            try
            {
                var query = new GetPermissionTreeQuery();
                var permissionTree = await _mediator.Send(query);
                return Ok(permissionTree);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}