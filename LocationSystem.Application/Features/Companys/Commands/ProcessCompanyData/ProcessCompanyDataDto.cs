using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Companys.Commands.ProcessCompanyData
{
    public class ProcessCompanyDataDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}
