using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Companys.Queries.GetProviceConpany
{
    public class GetProviceCompanyDto
    {
        public List<Tuple<string,int>>? ProviceConpany { get; set; }
    }
}
