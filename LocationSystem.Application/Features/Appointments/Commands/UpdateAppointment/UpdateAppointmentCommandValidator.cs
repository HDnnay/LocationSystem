using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Appointments.Commands.UpdateAppointment
{
    public  class UpdateAppointmentCommandValidator: AbstractValidator<DeleteAppointmentCommand>
    {
        public UpdateAppointmentCommandValidator() { }
    }
}
