using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Appointments.Queries.GetAppointmentDetail
{
    public class GetAppointmentDetailQuery:IRequest<AppointmentDetailDto>
    {
        public Guid Id { get; set; }
    }
}
