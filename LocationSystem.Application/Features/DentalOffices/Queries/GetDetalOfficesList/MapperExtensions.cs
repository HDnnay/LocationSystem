using LocationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.DentalOffices.Queries.GetDetalOfficesList
{
    public static class MapperExtensions
    {
        public static DentalOfficesListDto MapToDto(this DentalOffice model)
        {
            var result = new DentalOfficesListDto() { Id = model.Id,Name = model.Name };
            return result;
        }
    }
}
