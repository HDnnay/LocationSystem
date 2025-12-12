using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Patients.Queries.GetPatienList
{
    public class PatiensFilterDto
    {
        public int page { get; set; } = 1;
        public int pageSize { get; set; } = 10;
    }
}
