using FluentValidation;
using LocationSystem.Application.Features.Appointments.Commands.DeleteAppointment;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Appointments.Commands.UpdateAppointment
{
    public  class UpdateAppointmentCommandValidator: AbstractValidator<UpdateAppointmentCommand>
    {
        public UpdateAppointmentCommandValidator() 
        {
            RuleFor(t => t.EndDate).GreaterThan(t => t.StartDate).WithMessage("开始时间不能大于结束时间");
        }
    }
}
