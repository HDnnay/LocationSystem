using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.DentalOffices.Commands.CreateDentalOffice
{
    public class CreateDentalOfficesCommandValidator:AbstractValidator<CreateDentalOfficesCommand>
    {
        public CreateDentalOfficesCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name不能为空")
                .MaximumLength(20).WithMessage("Name长度不能超过20个字符");
        }
    }
}
