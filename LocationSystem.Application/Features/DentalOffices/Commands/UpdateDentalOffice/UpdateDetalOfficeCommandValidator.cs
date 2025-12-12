using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.DentalOffices.Commands.UpdateDentalOffice
{
    public class UpdateDetalOfficeCommandValidator:AbstractValidator<UpdateDetalOfficeCommand>
    {
        public UpdateDetalOfficeCommandValidator() 
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Name不能为空");
        }
    }
}
