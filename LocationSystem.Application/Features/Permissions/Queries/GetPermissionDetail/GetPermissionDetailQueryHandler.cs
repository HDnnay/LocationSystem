using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Dtos;
using LocationSystem.Application.Features.Permissions.Queries.GetPermissionDetail;
using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationSystem.Application.Features.Permissions.Queries.GetPermissionDetail
{
    public class GetPermissionDetailQueryHandler : IRequestHandler<GetPermissionDetailQuery, PermissionDto>
    {
        private readonly IPermissionRepository _permissionRepository;

        public GetPermissionDetailQueryHandler(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<PermissionDto> Handle(GetPermissionDetailQuery request)
        {
            // 获取权限及其角色和子权限
            var permission = await _permissionRepository.GetPermissionWithChildrenAsync(request.PermissionId);
            if (permission == null)
            {
                throw new Exception($"权限不存在，ID: {request.PermissionId}");
            }

            // 转换为DTO
            return new PermissionDto
            {
                Id = permission.Id,
                Name = permission.Name,
                Code = permission.Code,
                Description = permission.Description,
                ParentId = permission.ParentId,
                CreatedAt = permission.CreatedAt,
                UpdatedAt = permission.UpdatedAt,
                Roles = permission.Roles.Select(role => new RoleDto
                {
                    Id = role.Id,
                    Name = role.Name,
                    Code = role.Code,
                    Description = role.Description,
                    CreatedAt = role.CreatedAt,
                    UpdatedAt = role.UpdatedAt
                }).ToList(),
                ChildPermissions = permission.ChildPermissions.Select(cp => new PermissionDto
                {
                    Id = cp.Id,
                    Name = cp.Name,
                    Code = cp.Code,
                    Description = cp.Description,
                    ParentId = cp.ParentId,
                    CreatedAt = cp.CreatedAt,
                    UpdatedAt = cp.UpdatedAt,
                    ChildPermissions = cp.ChildPermissions.Select(ccp => new PermissionDto
                    {
                        Id = ccp.Id,
                        Name = ccp.Name,
                        Code = ccp.Code,
                        Description = ccp.Description,
                        ParentId = ccp.ParentId,
                        CreatedAt = ccp.CreatedAt,
                        UpdatedAt = ccp.UpdatedAt,
                        ChildPermissions = new List<PermissionDto>()
                    }).ToList()
                }).ToList()
            };
        }
    }
}