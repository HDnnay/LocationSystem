using FluentValidation;
using LocationSystem.Application.Features.Appointments.Commands.DeleteAppointment;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Appointments.Commands.UpdateAppointment
{
    public  class UpdateAppointmentCommandValidator: AbstractValidator<UpdateAppointmentCommand>
    {
        public UpdateAppointmentCommandValidator() { }
    }
}
