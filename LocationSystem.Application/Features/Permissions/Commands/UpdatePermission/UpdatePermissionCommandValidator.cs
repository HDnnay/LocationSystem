using FluentValidation;
using LocationSystem.Application.Features.Permissions.Commands.UpdatePermission;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Permissions.Commands.UpdatePermission
{
    public class UpdatePermissionCommandValidator : AbstractValidator<UpdatePermissionCommand>
    {
        public UpdatePermissionCommandValidator()
        {
            RuleFor(x => x.PermissionId)
                .NotEmpty().WithMessage("权限ID不能为空");

            RuleFor(x => x.PermissionDto.Name)
                .NotEmpty().WithMessage("权限名称不能为空")
                .MaximumLength(50).WithMessage("权限名称不能超过50个字符");

            RuleFor(x => x.PermissionDto.Code)
                .NotEmpty().WithMessage("权限代码不能为空")
                .MaximumLength(50).WithMessage("权限代码不能超过50个字符")
                .Matches("^[a-zA-Z0-9_:]+$", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                .WithMessage("权限代码只能包含字母、数字、下划线和冒号");

            RuleFor(x => x.PermissionDto.Description)
                .MaximumLength(200).WithMessage("权限描述不能超过200个字符");
        }
    }
}