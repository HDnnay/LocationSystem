using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Dtos.Interfaces
{
    public interface ICompanyEntity
    {
        public string? Address { get; set; }
        public string? Province { get; set; }
    }
}
