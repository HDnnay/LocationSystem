using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Patients.Queries.GetPatienDetail
{
    public class GetPatienDetailQuery:IRequset<PatienDetailDto>
    {
        public required Guid PatientId { get;set; }
    }
}
