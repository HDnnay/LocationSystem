using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Dtos;
using LocationSystem.Application.Features.Permissions.Commands.CreatePermission;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LocationSystem.Application.Features.Permissions.Commands.CreatePermission
{
    public class CreatePermissionCommandHandler : IRequsetHandler<CreatePermissionCommand, PermissionDto>
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreatePermissionCommandHandler(IPermissionRepository permissionRepository, IUnitOfWork unitOfWork)
        {
            _permissionRepository = permissionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PermissionDto> Handle(CreatePermissionCommand request)
        {
            // 检查权限名称是否已存在
            var existingPermission = await _permissionRepository.GetByNameAsync(request.PermissionDto.Name);
            if (existingPermission != null)
            {
                throw new Exception($"权限名称 {request.PermissionDto.Name} 已存在");
            }

            // 检查权限代码是否已存在
            existingPermission = await _permissionRepository.GetByCodeAsync(request.PermissionDto.Code);
            if (existingPermission != null)
            {
                throw new Exception($"权限代码 {request.PermissionDto.Code} 已存在");
            }

            // 创建权限
            var permission = new Permission(
                name: request.PermissionDto.Name,
                code: request.PermissionDto.Code,
                description: request.PermissionDto.Description
            );

            // 保存权限
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _permissionRepository.AddAsync(permission);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }

            // 返回DTO
            return new PermissionDto
            {
                Id = permission.Id,
                Name = permission.Name,
                Code = permission.Code,
                Description = permission.Description,
                CreatedAt = permission.CreatedAt,
                UpdatedAt = permission.UpdatedAt
            };
        }
    }
}