using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Dtos;
using LocationSystem.Application.Features.Permissions.Queries.GetPermissionTree;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationSystem.Application.Features.Permissions.Queries.GetPermissionTree
{
    public class GetPermissionTreeQueryHandler : IRequestHandler<GetPermissionTreeQuery, List<PermissionDto>>
    {
        private readonly IPermissionRepository _permissionRepository;

        public GetPermissionTreeQueryHandler(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<List<PermissionDto>> Handle(GetPermissionTreeQuery request)
        {
            var permissions = await _permissionRepository.GetPermissionTreeAsync();
            return MapPermissionsToDto(permissions);
        }

        private List<PermissionDto> MapPermissionsToDto(IEnumerable<Permission> permissions)
        {
            return permissions.Select(permission => new PermissionDto
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
                ChildPermissions = MapPermissionsToDto(permission.ChildPermissions)
            }).ToList();
        }
    }
}
