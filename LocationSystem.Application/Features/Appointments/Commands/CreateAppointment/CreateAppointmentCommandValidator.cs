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
            RuleFor(t => t.StartDate).GreaterThan(t => t.EndDate).WithMessage("开始时间不能大于结束时间");
        }
    }
}
