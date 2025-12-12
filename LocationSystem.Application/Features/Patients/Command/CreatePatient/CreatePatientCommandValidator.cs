using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Patients.Command.CreatePatient
{
    public class CreatePatientCommandValidator: AbstractValidator<CreatePatientCommand>
    {
        public CreatePatientCommandValidator()
        {
            RuleFor(x=>x.Name).NotEmpty().WithMessage("Name not null").MaximumLength(120);
            RuleFor(x => x.Email).NotEmpty().WithMessage("Name not null").MaximumLength(120);

        }
         
    }
}
