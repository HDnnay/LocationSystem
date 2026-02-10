using FluentValidation;
using LocationSystem.Application.Features.Roles.Commands.CreateRole;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Roles.Commands.CreateRole
{
    public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleCommandValidator()
        {
            RuleFor(x => x.RoleDto.Name)
                .NotEmpty().WithMessage("角色名称不能为空")
                .MaximumLength(50).WithMessage("角色名称不能超过50个字符");

            RuleFor(x => x.RoleDto.Code)
                .NotEmpty().WithMessage("角色代码不能为空")
                .MaximumLength(50).WithMessage("角色代码不能超过50个字符")
                .Matches("^[a-zA-Z0-9_]+$", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                .WithMessage("角色代码只能包含字母、数字和下划线");

            RuleFor(x => x.RoleDto.Description)
                .MaximumLength(200).WithMessage("角色描述不能超过200个字符");
        }
    }
}