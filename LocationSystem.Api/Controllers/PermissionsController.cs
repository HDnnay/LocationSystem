using LocationSystem.Application.Dtos;
using LocationSystem.Application.Features.Permissions.Commands.CreatePermission;
using LocationSystem.Application.Features.Permissions.Commands.DeletePermission;
using LocationSystem.Application.Features.Permissions.Commands.UpdatePermission;
using LocationSystem.Application.Features.Permissions.Queries.GetPermissionDetail;
using LocationSystem.Application.Features.Permissions.Queries.GetPermissionList;
using LocationSystem.Application.Features.Permissions.Queries.GetPermissionTree;
using LocationSystem.Application.Features.Permissions.Queries.GetPermissionTreeWithCheckStatus;
using LocationSystem.Application.Utilities;
using LocationSystem.Api.Filters;
using Microsoft.AspNetCore.Mvc;

using LocationSystem.Application.Features.Permissions.Queries.GetUserPermissionMenus;
using LocationSystem.Application.Features.Permissions.Queries.GetUserPermissions;

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
        [PermissionAuthorize("permission:view")]
        public async Task<IActionResult> GetPermissions([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyWord = "")
        {
            try
            {
                var query = new GetPermissionListQuery
                {
                    Page = page,
                    PageSize = pageSize,
                    KeyWord = keyWord
                };
                var permissions = await _mediator.Send(query);
                return Ok(permissions);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [PermissionAuthorize("permission:view")]
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
        [PermissionAuthorize("permission:create")]
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
        [PermissionAuthorize("permission:edit")]
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
                return BadRequest(ex);
            }
        }

        [HttpDelete("{id}")]
        [PermissionAuthorize("permission:delete")]
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
        [PermissionAuthorize("permission:view")]
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

        [HttpGet("tree-with-check")]
        [PermissionAuthorize("permission:view")]
        public async Task<IActionResult> GetPermissionTreeWithCheckStatus([FromQuery] Guid? roleId)
        {
            try
            {
                var query = new GetPermissionTreeWithCheckStatusQuery { RoleId = roleId };
                var permissionTree = await _mediator.Send(query);
                return Ok(permissionTree);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("user-menus")]
        public async Task<IActionResult> GetUserMenus()
        {
            try
            {
                // 从Token中获取用户ID
                var userId = User.GetUserId();
                if (userId == null)
                {
                    return Unauthorized();
                }

                var query = new GetUserPermissionMenusQuery
                {
                    UserId = userId.Value
                };

                var menus = await _mediator.Send(query);
                return Ok(menus);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("user-permissions")]
        public async Task<IActionResult> GetUserPermissions()
        {
            try
            {
                // 从Token中获取用户ID
                var userId = User.GetUserId();
                if (userId == null)
                {
                    return Unauthorized();
                }

                var query = new GetUserPermissionsQuery
                {
                    UserId = userId.Value
                };

                var permissions = await _mediator.Send(query);
                return Ok(permissions);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}