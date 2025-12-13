using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Dentists.Commands.CreateDentist
{
    public class CreateDentistCommandValidator: AbstractValidator<CreateDentistCommand>
    {
        public CreateDentistCommandValidator() 
        {
            RuleFor(t => t.Name).NotEmpty().WithMessage("Name不能为空").MaximumLength(24);
            RuleFor(t => t.Email).NotEmpty().WithMessage("Email不能为空")
                .EmailAddress().WithMessage("请输入正确的邮箱").MaximumLength(120) ;
        }
    }
}
