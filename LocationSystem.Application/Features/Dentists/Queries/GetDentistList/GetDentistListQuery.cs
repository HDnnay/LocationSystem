using LocationSystem.Application.Utilities;
using LocationSystem.Application.Utilities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Dentists.Queries.GetDentistList
{
    public class GetDentistListQuery: DentistListFilterDto, IRequset<PageResult<DentistListDto>>
    {
    }
}
