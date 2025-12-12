using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Patients.Command.UpdatePatient
{
    public class UpdatePatientCommandValidator: AbstractValidator<UpdatePatientCommand>
    {
        public UpdatePatientCommandValidator() 
        {
            RuleFor(t=>t.Id).NotEmpty();
            RuleFor(t=>t.Name).NotEmpty().MaximumLength(30);
            RuleFor(t=>t.Email).NotEmpty().MaximumLength(30);
        }
    }
}
