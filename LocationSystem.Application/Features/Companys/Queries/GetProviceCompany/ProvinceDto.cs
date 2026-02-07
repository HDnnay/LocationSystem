using LocationSystem.Application.Dtos.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Companys.Queries.GetProviceCompany
{
    public class ProvinceDto: ICompanyEntity
    {
        public string? Address { get; set; }
        public string? Province { get; set; }
    }
}
