using LocationSystem.Application.Features.Users.Commands.AssignRoles;
using LocationSystem.Application.Features.Users.Commands.CreateUser;
using LocationSystem.Application.Features.Users.Commands.DeleteUser;
using LocationSystem.Application.Features.Users.Commands.DisableUser;
using LocationSystem.Application.Features.Users.Commands.EnableUser;
using LocationSystem.Application.Features.Users.Commands.UpdateUser;
using LocationSystem.Application.Features.Users.Queries;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace LocationSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Users/types
        [HttpGet("types")]
        public IActionResult GetUserTypes()
        {
            try
            {
                var userTypes = new List<object>();
                foreach (UserType type in Enum.GetValues(typeof(UserType)))
                {
                    var displayAttribute = type.GetType().GetField(type.ToString())
                        .GetCustomAttributes(typeof(DisplayAttribute), false)
                        .FirstOrDefault() as DisplayAttribute;
                    
                    userTypes.Add(new
                    {
                        Value = (int)type,
                        Name = displayAttribute?.Name ?? type.ToString()
                    });
                }
                return Ok(userTypes);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/Users
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]GetAllUsersQuery query)
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

        // GET: api/Users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            try
            {
                var query = new GetUserByIdQuery { UserId = id };
                var user = await _mediator.Send(query);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/Users
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
        {
            try
            {
                var userId = await _mediator.Send(command);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/Users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserCommand command)
        {
            try
            {
                // 设置用户ID
                command.Id = id;

                // 执行命令
                var updatedUser = await _mediator.Send(command);

                // 返回更新后的用户
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/Users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                // 从当前用户信息中获取用户ID
                // 这里假设用户信息存储在HttpContext.User中
                var currentUserId = Guid.Parse(User.FindFirst("sub")?.Value ?? throw new Exception("当前用户未登录"));

                // 创建删除用户命令
                var command = new DeleteUserCommand { UserId = id, CurrentUserId = currentUserId };

                // 执行命令
                await _mediator.Send(command);

                // 返回成功
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/Users/{id}/roles
        [HttpPost("{id}/roles")]
        public async Task<IActionResult> AssignRoles(Guid id, [FromBody] AssignRolesCommand command)
        {
            try
            {
                // 设置用户ID
                command.UserId = id;

                // 执行命令
                await _mediator.Send(command);

                // 返回成功
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/Users/{id}/roles
        [HttpDelete("{id}/roles")]
        public async Task<IActionResult> RemoveRoles(Guid id)
        {
            try
            {
                // 创建分配角色命令，传入空角色列表
                var command = new AssignRolesCommand { UserId = id, RoleIds = new List<Guid>() };

                // 执行命令
                await _mediator.Send(command);

                // 返回成功
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/Users/{id}/disable
        [HttpPost("{id}/disable")]
        public async Task<IActionResult> DisableUser(Guid id)
        {
            try
            {
                var command = new DisableUserCommand { UserId = id };
                await _mediator.Send(command);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/Users/{id}/enable
        [HttpPost("{id}/enable")]
        public async Task<IActionResult> EnableUser(Guid id)
        {
            try
            {
                var command = new EnableUserCommand { UserId = id };
                await _mediator.Send(command);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
