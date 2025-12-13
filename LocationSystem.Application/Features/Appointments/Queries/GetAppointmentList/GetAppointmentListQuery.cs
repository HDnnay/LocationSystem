using LocationSystem.Application.Utilities;
using LocationSystem.Application.Utilities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Appointments.Queries.GetAppointmentList
{
    public class GetAppointmentListQuery: AppointmentListFilter,IRequset<PageResult<AppointmentListDto>>
    {
    }
}
