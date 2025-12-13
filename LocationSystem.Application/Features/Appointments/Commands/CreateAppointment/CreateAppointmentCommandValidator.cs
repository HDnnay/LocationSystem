using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Appointments.Commands.CreateAppointment
{
    public  class CreateAppointmentCommandValidator: AbstractValidator<CreateAppointmentCommand>
    {
        public CreateAppointmentCommandValidator()
        {
            RuleFor(t => t.EndDate).GreaterThan(t => t.StartDate).WithMessage("开始时间不能大于结束时间");
        }
    }
}
