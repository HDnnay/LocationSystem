using LocationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Dtos
{
    public class DentalOfficeDto
    {
        public Guid Id {get;set; }
        public string Name {get;set; } = null!;
        public DentalOfficeDto() { }
        public DentalOfficeDto(DentalOffice model)
        {
            Id = model.Id;
            Name = model.Name;
        }
    }
}
