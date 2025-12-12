using LocationSystem.Application.Utilities;
using LocationSystem.Application.Utilities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Patients.Queries.GetPatienList
{
    public class GetPatienListQuery: PatiensListFilterDto, IRequset<PageResult<PatienListDto>>
    {
    }
}
