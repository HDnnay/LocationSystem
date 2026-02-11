using LocationSystem.Application.Features.Permissions.Models;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities;

namespace LocationSystem.Application.Features.Permissions.Commands.CreatePermissionMenu
{
    public class CreatePermissionMenuCommandHandler : IRequestHandler<CreatePermissionMenuCommand, PermissionMenuDto>
    {
        public async Task<PermissionMenuDto> Handle(CreatePermissionMenuCommand request)
        {
            // 创建权限
            var permission = new Permission(
                name: request.Name,
                code: request.Code,
                description: request.Description
            );

            // 由于没有实际的存储库，这里直接返回DTO
            // 实际项目中应该使用IPermissionRepository和IUnitOfWork来保存权限

            // 返回DTO
            return new PermissionMenuDto
            {
                Id = permission.Id,
                Name = permission.Name,
                Code = permission.Code,
                Description = permission.Description,
                ParentId = null,
                IsMenu = false,
                MenuPath = string.Empty,
                MenuIcon = string.Empty,
                Order = 0,
                CreatedAt = permission.CreatedAt,
                UpdatedAt = permission.UpdatedAt ?? permission.CreatedAt
            };
        }
    }
}
